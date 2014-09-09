using System;
using System.Collections.Generic;
using System.Diagnostics;
using DK2D.Map;
using DK2D.Objects;
using DK2D.Objects.Creatures;
using DK2D.Terrains;
using SFML.Graphics;
using SFML.Window;

namespace DK2D
{
    internal class Game
    {
        private const int WindowWidth = MapWidth * CellWidth;
        private const int WindowHeight = MapHeight * CellHeight;

        private const int MapWidth = 25;
        private const int MapHeight = 18;

        public const int CellWidth = 32;
        public const int CellHeight = 32;

        private readonly RenderWindow _window;
        private readonly Map.Map _map;

        private readonly List<GameObject> _gameObjects = new List<GameObject>();

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
                    _gameObjects.Add(new Imp { Position = coords });
                }
            }
        }

        private void Update(float secondsElapsed)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Update(secondsElapsed, this);
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
                        _window.Draw(
                            new RectangleShape
                                {
                                    Position = new Vector2f(x * CellWidth, y * CellHeight),
                                    Size = new Vector2f(CellWidth, CellHeight),
                                    FillColor = cell.OverlayColor.Value
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
        }
    }
}