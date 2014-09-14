using System.Diagnostics;

using DK2D.Map;

using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects
{
    internal class GameObject
    {
        private Vector2f _position;
        private MapCell _cell;

        public GameObject(Game game)
        {
            Game = game;
        }

        public float BoundingRadius { get; protected set; }

        public MapCell Cell
        {
            get
            {
                return _cell;
            }

            private set
            {
                if (value != _cell)
                {
                    if (_cell != null)
                    {
                        bool removed = _cell.Objects.Remove(this);
                        Debug.Assert(removed, "Attempt to remove object that did not exist in cell");
                    }
                    
                    _cell = value;

                    if (_cell != null)
                    {
                        _cell.Objects.Add(this);
                    }
                }
            }
        }

        public Vector2f Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;

                Cell = Game.Map.MapCoordsToCell(Position);
            }
        }

        public Color Color { get; protected set; }

        protected Game Game { get; private set; }

        public virtual void Update(float secondsElapsed)
        {
        }
    }
}
