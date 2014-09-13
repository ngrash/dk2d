using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using DK2D.Map;
using DK2D.Objects;
using DK2D.Objects.Creatures;
using DK2D.Terrains;
using DK2D.UI;

using SFML.Graphics;
using SFML.Window;

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
        private readonly Map.Map _map;

        private readonly List<GameObject> _gameObjects = new List<GameObject>();

        private readonly List<Menu> _menus = new List<Menu>
            {
                new TerrainMenu { Position = new Vector2f(10, WindowHeight - 60) }
            };

        private Button _activeButton;

        public Game()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "DK2D");
            _window.Closed += WindowOnClosed;
            _window.MouseButtonReleased += WindowOnMouseButtonReleased;

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
                    if (mouseButtonEventArgs.X > menu.Position.X
                        && mouseButtonEventArgs.X < menu.Position.X + menu.Size.X
                        && mouseButtonEventArgs.Y > menu.Position.Y
                        && mouseButtonEventArgs.Y < menu.Position.Y + menu.Size.Y)
                    {
                        Button button = menu.HitTest(new Vector2f(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));

                        if (button != null)
                        {
                            button.IsActive = true;

                            if (_activeButton != null)
                            {
                                _activeButton.IsActive = false;
                            }

                            _activeButton = button;
                        }

                        return;
                    }
                }

                MapCell cell = _map.MapCoordsToCell(new Vector2f(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));

                if (_activeButton != null)
                {
                    _activeButton.CellClicked(cell);
                    return;
                }
                
                if (cell.Terrain is ClaimedPath)
                {
                    Vector2f coords = _window.MapPixelToCoords(new Vector2i(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));

                    if (_gameObjects.Count == 0)
                    {
                        _gameObjects.Add(new Imp { Position = coords });
                    }
                    else
                    {
                        Imp imp = _gameObjects.OfType<Imp>().Single();
                        Vector2i cellIndex = _map.MapCoordsToCellIndex(coords);

                        imp.MoveTo(cellIndex, this);
                    }
                }
            }
        }

        private void Update(float secondsElapsed)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Update(secondsElapsed, this);
            }

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    MapCell cell = _map[x, y];
                    cell.Update(secondsElapsed);
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

            // Draw game objects
            _display.DrawObjects(_gameObjects);

            // Imp debug drawings
            _display.DrawImpDebugInfos(_gameObjects.OfType<Imp>());

            // Draw menu
            _display.DrawMenus(_menus);
        }
    }
}