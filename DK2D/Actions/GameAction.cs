using System;
using System.Diagnostics;

using DK2D.Map;
using DK2D.Objects.Creatures;

namespace DK2D.Actions
{
    internal class GameAction
    {
        private MapCell _cell;

        private bool _isDone;

        public MapCell Cell
        {
            get
            {
                return _cell;
            }

            set
            {
                if (value != _cell)
                {
                    if (_cell != null)
                    {
                        bool removed = _cell.Actions.Remove(this);
                        Debug.Assert(removed, "Attempt to remove action that did not exist in cell");
                    }

                    _cell = value;

                    if (_cell != null)
                    {
                        _cell.Actions.Add(this);
                    }
                }
            }
        }

        public Creature Creature { get; set; }

        public bool IsDone
        {
            get
            {
                return _isDone;
            }

            set
            {
                if (_isDone)
                {
                    throw new InvalidOperationException("Cannot set IsDone twice");
                }

                _isDone = true;
            }
        }

        public virtual void Perform(float secondsElapsed)
        {
            IsDone = true;
        }
    }
}
