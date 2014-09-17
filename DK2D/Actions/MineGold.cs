using DK2D.Map;
using DK2D.Rooms;
using DK2D.Terrains;

namespace DK2D.Actions
{
    internal class MineGold : LaborGameAction
    {
        public MineGold()
        {
            Duration = 0.5f;
        }

        public MapCell Target { get; set; }

        protected override void Completed()
        {
            int minedGold = 50;
            var goldSeam = (GoldSeam)Target.Terrain;
            if (goldSeam.Quantity > minedGold)
            {
                goldSeam.Quantity -= minedGold;
            }
            else
            {
                minedGold = goldSeam.Quantity;
                goldSeam.Quantity = 0;
                Target.Terrain = new DirtPath();
            }

            MapCell treasury = Creature.Cell.FindNearest<Treasury>();
            if (treasury != null)
            {
                Creature.Gold += minedGold;
                Creature.MoveTo(new StoreGold { Cell = treasury, Creature = Creature });
            }
            else
            {
                // TODO: Drop gold
            }
        }
    }
}
