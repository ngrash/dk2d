using DK2D.Rooms;

namespace DK2D.Map
{
    class MapCell
    {
        public bool IsClaimed { get; set; }

        public Terrain Terrain { get; set; }

        public Room Room { get; set; }


    }
}
