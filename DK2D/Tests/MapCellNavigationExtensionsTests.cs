using System.Collections.Generic;

using DK2D.Map;

using NUnit.Framework;

namespace DK2D.Tests
{
    [TestFixture]
    internal class MapCellNavigationExtensionsTests
    {
        [Test]
        public void ItGetsCorrectAdjacents()
        {
            var map = NewTestMap();
            MapCell cell = map[5, 5];
            IEnumerable<MapCell> cells = cell.Adjacents();
            Assert.That(cells, Is.SubsetOf(new[] { map[5, 4], map[6, 5], map[5, 6], map[4, 5] }));
        }

        [Test]
        public void ItCalculatesDistanceOf1()
        {
            var map = NewTestMap();
            Assert.That(map[0, 0].DistanceTo(map[0, 1]), Is.EqualTo(1));
        }

        [Test]
        public void ItCalculatesDistanceOf2()
        {
            var map = NewTestMap();
            Assert.That(map[0, 0].DistanceTo(map[0, 2]), Is.EqualTo(2));
        }

        private static Map.Map NewTestMap()
        {
            return new Map.Map(1, 1, 10, 10);
        }
    }
}
