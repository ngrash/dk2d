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
        private readonly Map.Map _map;

        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly Button[] _buttons = new[]
            {
                new Button { Color = Colors.Earth, Position = new Vector2f(25, WindowHeight - 50), Size = new Vector2f(30, 30) }
            };

        public Game()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "DK2D");
            _window.Closed += WindowOnClosed;
            _window.MouseButtonReleased += WindowOnMouseButtonReleased;
            _map = new Map.Map(CellWidth, CellHeight, WindowWidth / CellWidth, WindowHeight / CellHeight);

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    _map[x, y].Terrain = new Earth();
                }
            }
        }

        public Map.Map Map { get { return _map; }}

        public void Run()
        {
            Stopwatch timer = Stopwatch.StartNew();

            while (_window.IsOpen())
            {
                var secondsElapsed = (float) timer.Elapsed.TotalSeconds;
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
            if (mouseButtonEventArgs.Button == Mouse.Button.Left)
            {
                MapCell cell = _map.MapCoordsToCell(new Vector2f(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));

                if (cell.Terrain is Earth)
                {
                    cell.Terrain = new DirtPath();
                }
                else if (cell.Terrain is DirtPath)
                {
                    cell.Terrain = new ClaimedPath();
                }
                else if (cell.Terrain is ClaimedPath)
                {
                    Vector2f coords = _window.MapPixelToCoords(new Vector2i(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));

                    if (_gameObjects.Count == 0)
                    {
                        _gameObjects.Add(new Imp { Position = coords });
                    }
                    else
                    {
                        _gameObjects.OfType<Imp>().Single().Path.Add(coords);
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
            for (int x = 0; x <= WindowWidth/CellWidth; x++)
            {
                for (int y = 0; y <= WindowHeight/CellHeight; y++)
                {
                    target.Draw(new RectangleShape
                        {
                            Position = new Vector2f(x * CellWidth, y * CellHeight),
                            Size = new Vector2f(CellWidth, CellHeight),
                            FillColor = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? new Color(42, 42, 42) : new Color(23, 23, 23)
                        });
                }
            }

            // Draw cells
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    MapCell cell = _map[x, y];
                    _window.Draw(new RectangleShape
                        {
                            Position = new Vector2f(x * CellWidth, y * CellHeight),
                            Size = new Vector2f(CellWidth, CellHeight),
                            FillColor = cell.Terrain.Color
                        });

                    // Draw overlay
                    if (cell.OverlayColor.HasValue)
                    {
                        Color color = cell.OverlayColor.Value;

                        if (cell.IsOverlayFadeEnabled)
                        {
                            color.A = (byte)(color.A * (1 - cell.OverlayFade));
                        }

                        _window.Draw(
                            new RectangleShape
                                {
                                    Position = new Vector2f(x * CellWidth, y * CellHeight),
                                    Size = new Vector2f(CellWidth, CellHeight),
                                    FillColor = color
                                });
                    }
                }
            }

            // Draw game objects
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Sprite.Position = gameObject.Position;
                _window.Draw(gameObject.Sprite);
            }

            // Imp debug drawings
            foreach (var imp in _gameObjects.OfType<Imp>())
            {
                // Draw path
                var vertices = new VertexArray(PrimitiveType.Lines);
                vertices.Append(new Vertex(imp.Position) { Color = Colors.DebugPath });

                for (int index = 0; index < imp.Path.Count; index++)
                {
                    Vector2f position = imp.Path[index];
                    if (index != 0)
                    {
                        vertices.Append(new Vertex(imp.Path[index - 1]) { Color = Colors.DebugPath });
                    }

                    vertices.Append(new Vertex(position) { Color = Colors.DebugPath });
                }

                vertices.Draw(_window, RenderStates.Default);

                // Draw scan radius
                _window.Draw(new CircleShape(Imp.ScanRadius * CellWidth)
                    {
                        Position = new Vector2f(imp.Position.X - (Imp.ScanRadius * CellWidth), imp.Position.Y - (Imp.ScanRadius * CellWidth)),
                        FillColor = Color.Transparent,
                        OutlineColor = Colors.DebugScanRadius,
                        OutlineThickness = 3
                    });
            }

            // Draw menu
            foreach (Button button in _buttons)
            {
                _window.Draw(new RectangleShape
                    {
                        OutlineColor = Color.Black,
                        OutlineThickness = 5,
                        Position = button.Position,
                        FillColor = button.Color,
                        Size = button.Size
                    });
            }
        }
    }
}