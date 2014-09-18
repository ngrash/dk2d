using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using DK2D.Map;
using DK2D.Objects;
using DK2D.Objects.Creatures;
using DK2D.Terrains;
using DK2D.UI;

using SFML.Graphics;
using SFML.Window;

using DialogResult = System.Windows.Forms.DialogResult;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace DK2D
{
    internal class Game
    {
        public const int CellWidth = 32;
        public const int CellHeight = 32;

        private const int MapWidth = 25;
        private const int MapHeight = 18;

        private const int WindowWidth = MapWidth * CellWidth;
        private const int WindowHeight = MapHeight * CellHeight;

        private readonly RenderWindow _window;
        private readonly Display _display;

        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly List<GameObject> _hand = new List<GameObject>();

        private readonly List<Menu> _menus = new List<Menu>
            {
                new TerrainMenu { Position = new Vector2f(10, WindowHeight - 60) },
                new RoomMenu { Position = new Vector2f(300, WindowHeight - 60) }
            };

        private Map.Map _map;

        private Button _activeButton;

        private Vector2f _mouseCoords;

        private MapCell _cellSelectionStart;
        private MapCell _cellUnderMouse;
        private GameObject _objectUnderMouse;

        public Game()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "DK2D");
            _window.Closed += WindowOnClosed;
            _window.MouseButtonReleased += WindowOnMouseButtonReleased;
            _window.MouseButtonPressed += WindowOnMouseButtonPressed;
            _window.MouseMoved += WindowOnMouseMoved;
            _window.KeyPressed += WindowOnKeyPressed;

            _display = new Display(_window, new Vector2i(CellWidth, CellHeight));

            _map = new Map.Map(CellWidth, CellHeight, WindowWidth / CellWidth, WindowHeight / CellHeight);

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    _map[x, y].Terrain = new Earth();
                }
            }
        }

        public Map.Map Map
        {
            get
            {
                return _map;
            }
        }

        public void Run()
        {
            Stopwatch timer = Stopwatch.StartNew();

            while (_window.IsOpen())
            {
                var secondsElapsed = (float)timer.Elapsed.TotalSeconds;
                timer.Restart();

                _window.DispatchEvents();

                Update(secondsElapsed);

                Draw(_window);

                _window.Display();
            }
        }

        private void WindowOnClosed(object sender, EventArgs eventArgs)
        {
            Textures.Release();

            _window.Close();
        }

        private void WindowOnKeyPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Control && keyEventArgs.Code == Keyboard.Key.O)
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "DK2D Map (*.map)|*.map";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _gameObjects.Clear();
                        _cellSelectionStart = null;
                        _activeButton = null;

                        string file = openFileDialog.FileName;
                        _map = MapFile.Load(file);
                    }
                }
            }
            else if (keyEventArgs.Control && keyEventArgs.Code == Keyboard.Key.S)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "DK2D Map (*.map)|*.map";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string file = saveFileDialog.FileName;
                        MapFile.Save(_map, file);
                    }
                }
            }
        }

        private void WindowOnMouseMoved(object sender, MouseMoveEventArgs mouseMoveEventArgs)
        {
            _mouseCoords = _window.MapPixelToCoords(new Vector2i(mouseMoveEventArgs.X, mouseMoveEventArgs.Y));
            _cellUnderMouse = _map.MapCoordsToCell(_mouseCoords);
        }

        private void WindowOnMouseButtonPressed(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.Button == Mouse.Button.Left)
            {
                if (_objectUnderMouse is Creature)
                {
                    _gameObjects.Remove(_objectUnderMouse);
                    _hand.Add(_objectUnderMouse);
                }
                else
                {
                    _cellSelectionStart = _map.MapCoordsToCell(_mouseCoords);
                }
            }
            else if (mouseButtonEventArgs.Button == Mouse.Button.Right)
            {
                if (_hand.Count > 0)
                {
                    GameObject gameObject = _hand[0];
                    _hand.Remove(gameObject);

                    gameObject.Position = _mouseCoords;
                    _gameObjects.Add(gameObject);
                }
            }
        }

        private void WindowOnMouseButtonReleased(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.Button == Mouse.Button.Right)
            {
                if (_activeButton != null)
                {
                    _activeButton.IsActive = false;
                    _activeButton = null;
                }
            }

            if (mouseButtonEventArgs.Button == Mouse.Button.Left)
            {
                foreach (Menu menu in _menus)
                {
                    Button clickedButton = menu.HitTest(new Vector2f(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));
                    if (clickedButton != null)
                    {
                        if (_activeButton != null)
                        {
                            _activeButton.IsActive = false;
                        }

                        _activeButton = clickedButton;
                        _activeButton.IsActive = true;

                        _cellSelectionStart = null;

                        return;
                    }
                }

                // Apply selection
                if (_cellUnderMouse != null)
                {
                    if (_cellSelectionStart == null)
                    {
                        _cellSelectionStart = _cellUnderMouse;
                    }

                    bool isSelecting = !_cellSelectionStart.IsSelected;

                    int minX = Math.Min(_cellSelectionStart.X, _cellUnderMouse.X);
                    int maxX = Math.Max(_cellSelectionStart.X, _cellUnderMouse.X);

                    int minY = Math.Min(_cellSelectionStart.Y, _cellUnderMouse.Y);
                    int maxY = Math.Max(_cellSelectionStart.Y, _cellUnderMouse.Y);

                    for (int x = minX; x <= maxX; x++)
                    {
                        for (int y = minY; y <= maxY; y++)
                        {
                            MapCell cell = _map[x, y];
                            if (_activeButton == null)
                            {
                                cell.IsSelected = cell.IsPenetrable && isSelecting;
                            }
                            else
                            {
                                _activeButton.CellClicked(cell);
                            }
                        }
                    }

                    _cellSelectionStart = null;
                }
            }
        }

        private void Update(float secondsElapsed)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Update(secondsElapsed);
            }

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    MapCell cell = _map[x, y];
                    cell.Update(secondsElapsed);
                }
            }

            // Update selected object, this cannot happen in the mouse move event, because we need to consider moving objects
            _objectUnderMouse = null;
            if (_cellUnderMouse != null)
            {
                foreach (GameObject gameObject in _cellUnderMouse.Objects)
                {
                    float distance = gameObject.Position.DistanceTo(_mouseCoords);
                    if (distance < gameObject.BoundingRadius)
                    {
                        _objectUnderMouse = gameObject;
                        break;
                    }
                }
            }
        }

        private void Draw(RenderTarget target)
        {
            target.Clear();

            // Draw background
            _display.DrawBackground();

            // Draw cells
            _display.DrawCells(_map);

            // Draw selection preview
            if (_cellUnderMouse != null)
            {
                _display.DrawSelectionPreview(_map, _cellSelectionStart, _cellUnderMouse);
            }

            // Draw game objects
            _display.DrawObjects(_gameObjects);

            // Imp debug drawings
            _display.DrawImpDebugInfos(_gameObjects.OfType<Imp>());

            // Draw menu
            _display.DrawMenus(_menus);

            // Draw bounding circle around object the mouse is over
            if (_objectUnderMouse != null)
            {
                target.Draw(new CircleShape(_objectUnderMouse.BoundingRadius)
                    {
                        Position = _objectUnderMouse.Position - new Vector2f(_objectUnderMouse.BoundingRadius, _objectUnderMouse.BoundingRadius),
                        FillColor = Color.Transparent,
                        OutlineColor = Color.Red,
                        OutlineThickness = 1
                    });
            }

            // Draw information about the current hoovered cell
            if (_cellUnderMouse != null)
            {
                var info = new StringBuilder();
                info.Append(_cellUnderMouse.Terrain.GetType().Name);
                
                if (_cellUnderMouse.Objects.Count > 0)
                {
                    info.Append(": ");
                    string objects = string.Join(", ", _cellUnderMouse.Objects.Select(o => o.GetType().Name));
                    info.Append(objects);
                }

                target.Draw(new Text(info.ToString(), new Font("Assets/Font/PureEvil.ttf")) { Position = new Vector2f(10, 10), Color = Color.Black });
            }
        }
    }
}