using System;
using System.Diagnostics;
using DK2D.Map;
using SFML.Graphics;
using SFML.Window;

namespace DK2D
{
    internal class Game
    {
        private const int WindowWidth = 800;
        private const int WindowHeight = 600;

        private const int TileWidth = 32;
        private const int TileHeight = 32;

        private readonly RenderWindow _window;
        private readonly Map.Map _map;

        public Game()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "DK2D");
            _window.Closed += (sender, args) => _window.Close();
            _window.MouseButtonReleased += WindowOnMouseButtonReleased;
            _map = new Map.Map(TileWidth, TileHeight, WindowWidth / TileWidth, WindowHeight / TileHeight);
        }

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

        private void WindowOnMouseButtonReleased(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.Button == Mouse.Button.Left)
            {
                MapCell cell = _map.MapCoordsToCell(new Vector2f(mouseButtonEventArgs.X, mouseButtonEventArgs.Y));
                cell.IsClaimed = !cell.IsClaimed;
            }
        }

        private void Update(float secondsElapsed)
        {
        }

        private void Draw(RenderTarget target)
        {
            target.Clear();

            // Draw background
            for (int x = 0; x <= WindowWidth/TileWidth; x++)
            {
                for (int y = 0; y <= WindowHeight/TileHeight; y++)
                {
                    target.Draw(new RectangleShape
                        {
                            Position = new Vector2f(x * TileWidth, y * TileHeight),
                            Size = new Vector2f(TileWidth, TileHeight),
                            FillColor = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? new Color(42, 42, 42) : new Color(23, 23, 23)
                        });
                }
            }

            // Draw cells
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if (_map[x, y].IsClaimed)
                    {
                        target.Draw(new RectangleShape
                            {
                                Position = new Vector2f(x * TileWidth, y * TileHeight),
                                Size = new Vector2f(TileWidth, TileHeight),
                                FillColor = new Color(23, 23, 255, 100)
                            });
                    }
                }
            }
        }
    }
}