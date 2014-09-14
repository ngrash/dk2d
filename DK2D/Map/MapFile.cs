using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using DK2D.Terrains;

namespace DK2D.Map
{
    [Serializable]
    public class MapFile
    {
        private static readonly Dictionary<int, Type> TerrainMap = new Dictionary<int, Type>
            {
                { 0, typeof(Earth) },
                { 1, typeof(DirtPath) },
                { 2, typeof(ClaimedPath) },
                { 3, typeof(GoldSeam) }
            };

        private MapFile()
        {    
        }

        public int[,] Terrains { get; set; }

        internal static Map Load(string path)
        {
            MapFile mapFile;

            using (FileStream file = File.OpenRead(path))
            {
                var bf = new BinaryFormatter();
                mapFile = (MapFile)bf.Deserialize(file);
            }

            int width = mapFile.Terrains.GetLength(0);
            int height = mapFile.Terrains.GetLength(1);

            Map map = new Map(Game.CellWidth, Game.CellHeight, width, height);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map[x, y].Terrain = MapTerrain(mapFile.Terrains[x, y]);
                }
            }

            return map;
        }

        internal static void Save(Map map, string path)
        {
            var mapFile = new MapFile();
            mapFile.Terrains = new int[map.Width, map.Height];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    mapFile.Terrains[x, y] = MapTerrain(map[x, y].Terrain);
                }
            }

            using (FileStream file = File.OpenWrite(path))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, mapFile);
            }
        }

        private static int MapTerrain(Terrain terrain)
        {
            return TerrainMap.Single(kv => terrain.GetType() == kv.Value).Key;
        }

        private static Terrain MapTerrain(int terrain)
        {
            Type type = TerrainMap[terrain];
            return (Terrain)Activator.CreateInstance(type);
        }
    }
}
