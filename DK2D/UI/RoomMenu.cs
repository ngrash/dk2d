using DK2D.Rooms;

namespace DK2D.UI
{
    internal class RoomMenu : Menu
    {
        public RoomMenu()
        {
            Add(new Treasury());
        }

        private void Add(Room room)
        {
            Add(new RoomButton(room));
        }
    }
}
