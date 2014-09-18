using System;
using System.Collections.Generic;

using DK2D.Map;
using DK2D.Objects;
using DK2D.Objects.Creatures;
using DK2D.UI;

using SFML.Graphics;
using SFML.Window;

namespace DK2D
{
    internal class Display
    {
        private readonly RenderTarget _target;
        private readonly Vector2i _cellSize;

        public Display(RenderTarget target, Vector2i cellSize)
        {
            _target = target;
            _cellSize = cellSize;
        }

        public void DrawBackground()
        {
            for (int x = 0; x <= _target.Size.X / _cellSize.X; x++)
            {
                for (int y = 0; y <= _target.Size.Y / _cellSize.Y; y++)
                {
                    _target.Draw(new RectangleShape
                    {
                        Position = new Vector2f(x * _cellSize.X, y * _cellSize.Y),
                        Size = new Vector2f(_cellSize.X, _cellSize.Y),
                        FillColor = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? new Color(42, 42, 42) : new Color(23, 23, 23)
                    });
                }
            }
        }

        public void DrawCells(Map.Map map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var position = new Vector2f(x * _cellSize.X, y * _cellSize.Y);
                    var size = new Vector2f(_cellSize.X, _cellSize.Y); 

                    MapCell cell = map[x, y];

                    if (cell.Terrain != null)
                    {
                        if (cell.Terrain.Texture != null)
                        {
                            _target.Draw(new Sprite(cell.Terrain.Texture) { Position = position });
                        }
                        else
                        {
                            _target.Draw(
                                new RectangleShape { Position = position, Size = size, FillColor = cell.Terrain.Color });
                        }
                    }

                    if (cell.Room != null)
                    {
                        _target.Draw(new RectangleShape { Position = position, Size = size, FillColor = Color.Green});
                    }

                    if (cell.IsSelected)
                    {
                        _target.Draw(new RectangleShape
                            {
                                Position = new Vector2f(x * _cellSize.X, y * _cellSize.Y),
                                Size = new Vector2f(_cellSize.X, _cellSize.Y),
                                FillColor = Colors.SelectedCell
                            });
                    }

                    // Draw overlay
                    if (cell.OverlayColor.HasValue)
                    {
                        Color color = cell.OverlayColor.Value;

                        if (cell.IsOverlayFadeEnabled)
                        {
                            color.A = (byte)(color.A * (1 - cell.OverlayFade));
                        }

                        _target.Draw(
                            new RectangleShape
                            {
                                Position = new Vector2f(x * _cellSize.X, y * _cellSize.Y),
                                Size = new Vector2f(_cellSize.X, _cellSize.Y),
                                FillColor = color
                            });
                    }
                }
            }
        }

        public void DrawObjects(IEnumerable<GameObject> objects)
        {
            foreach (GameObject gameObject in objects)
            {
                _target.Draw(new CircleShape(gameObject.BoundingRadius)
                    {
                        FillColor = gameObject.Color,
                        Position = gameObject.Position - new Vector2f(gameObject.BoundingRadius, gameObject.BoundingRadius)
                    });
            }
        }

        public void DrawImpDebugInfos(IEnumerable<Imp> imps)
        {
            foreach (Imp imp in imps)
            {
                //DrawImpDebugInfo(imp);
            }
        }

        public void DrawImpDebugInfo(Imp imp)
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

            vertices.Draw(_target, RenderStates.Default);

            // Draw scan radius
            _target.Draw(new CircleShape(Imp.ScanRadius * _cellSize.X)
            {
                Position = new Vector2f(imp.Position.X - (Imp.ScanRadius * _cellSize.X), imp.Position.Y - (Imp.ScanRadius * _cellSize.X)),
                FillColor = Color.Transparent,
                OutlineColor = Colors.DebugScanRadius,
                OutlineThickness = 3
            });
        }

        public void DrawMenus(IEnumerable<Menu> menus)
        {
            foreach (Menu menu in menus)
            {
                DrawMenu(menu);
            }
        }

        public void DrawMenu(Menu menu)
        {
            _target.Draw(new RectangleShape
            {
                OutlineThickness = 1,
                OutlineColor = Colors.MenuOutline,
                FillColor = Colors.MenuBackground,
                Position = menu.Position,
                Size = menu.Size
            });

            foreach (Button button in menu.Buttons)
            {
                _target.Draw(new RectangleShape
                {
                    OutlineColor = Colors.MenuOutline,
                    OutlineThickness = button.IsActive ? 3 : 1,
                    Position = button.Position + menu.Position,
                    FillColor = button.Color,
                    Size = button.Size
                });
            }
        }

        public void DrawSelectionPreview(Map.Map map, MapCell start, MapCell mouseOver)
        {
            if (start == null || start == mouseOver)
            {
                _target.Draw(new RectangleShape
                    {
                        Position = map.MapCellIndexToCoords(mouseOver.Position),
                        Size = new Vector2f(_cellSize.X, _cellSize.Y),
                        FillColor = Color.Transparent,
                        OutlineColor = Colors.SelectedCell,
                        OutlineThickness = 1
                    });
            }
            else
            {
                int minX = Math.Min(start.X, mouseOver.X);
                int maxX = Math.Max(start.X, mouseOver.X);
                int minY = Math.Min(start.Y, mouseOver.Y);
                int maxY = Math.Max(start.Y, mouseOver.Y);

                Vector2f startCoords = map.MapCellIndexToCoords(new Vector2i(minX, minY));
                Vector2f endCoords = map.MapCellIndexToCoords(new Vector2i(maxX, maxY)) + new Vector2f(_cellSize.X, _cellSize.Y);

                _target.Draw(new RectangleShape
                {
                    Position = startCoords,
                    Size = endCoords - startCoords,
                    FillColor = Color.Transparent,
                    OutlineColor = Colors.SelectedCell,
                    OutlineThickness = 1
                });
            }
        }
    }
}
