using System;
using VoxelRPG.Libs.Noise;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums.Chunk;

namespace VoxelRPG.Game.Generation
{
    public class WorldGenerator
    {
        FastNoise generator;

        public WorldGenerator()
        {
            generator = new FastNoise(123); //TODO: Seed
            generator.SetFrequency(0.02f); // größer = steiler, kleiner = flacher
        }

        public int GetHeight(int x, int z)
        {
            float height = GeneratorUtility.Map(0, 30, -1, 1, generator.GetPerlin(x, z));
            return (int)height;
        }

        public BlockType GetBlockType(Vector3Int pos, int height)
        {
            if (pos.Y > height)
                return BlockType.AIR;
            else if (pos.Y > 20)
                return BlockType.SNOW;
            else if (pos.Y > 5)
                return BlockType.GRASS;
            else
                return BlockType.STONE;

        }
    }
}
