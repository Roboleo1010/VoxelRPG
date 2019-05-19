using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelRPG.Libs.Noise;

namespace VoxelRPG.Game.Generation
{
    public class WorldGeneration
    {
        FastNoise generator;

        public WorldGeneration()
        {
            generator = new FastNoise(123); //TODO: Seed
            generator.SetFrequency(0.03f); // größer = steiler, kleiner = flacher
        }

        public int GetHeight(int x, int z)
        {
            float height = GeneratorUtility.Map(0, 15, -1, 1, generator.GetPerlin(x, z));
            return (int)height;
        }
    }
}
