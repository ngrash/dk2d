using System;

using DK2D.Rooms;

namespace DK2D.UI
{
    internal class RoomButton : Button
    {
        private readonly Room _roomPrototype;

        public RoomButton(Room roomPrototype)
        {
            _roomPrototype = roomPrototype;

            // TODO: Color = _roomPrototype.Color;
        }

        public override void CellClicked(Map.MapCell cell)
        {
            cell.Room = (Room)Activator.CreateInstance(_roomPrototype.GetType());
        }
    }
}