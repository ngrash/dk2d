using System.Collections.Generic;
using System.Linq;

using DK2D.Map;
using DK2D.Pathfinding;
using DK2D.Terrains;

using NUnit.Framework;

using SFML.Window;

namespace DK2D.Tests
{
    [TestFixture]
    internal class AStarTests
    {
        [Test]
        public void FindsAShortWay()
        {
            var map = new Map.Map(1, 1, 10, 10);

            var star = new AStar(i => true, i => map.Get(i).Adjacents().Select(cell => cell.Position));
            List<Vector2i> path = star.FindPath(V(5, 5), V(5, 8));

            Assert.That(path, Is.EqualTo(new[] { V(5, 5), V(5, 6), V(5, 7), V(5, 8) }));
        }

        [Test]
        public void FindsWayAroundAbstacle()
        {
            var map = new Map.Map(1, 1, 10, 10);
            map[5, 6].Terrain = new Lava();
            map[6, 6].Terrain = new Lava();

            var star = new AStar(i => !(map.Get(i).Terrain is Lava), i => map.Get(i).Adjacents().Select(cell => cell.Position));
            List<Vector2i> path = star.FindPath(V(5, 5), V(5, 8));

            Assert.That(new[] { V(5, 5), V(4, 5), V(4, 6), V(4, 7), V(5, 8) }, Is.SubsetOf(path));
            Assert.That(path.Count, Is.EqualTo(6));
        }

        private static Vector2i V(int x, int y)
        {
            return new Vector2i(x, y);
        }
    }
}
