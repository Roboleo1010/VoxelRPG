using VoxelRPG.Libs.Noise;

namespace VoxelRPG.Game.Generation
{
    public class WorldGenerator
    {
        FastNoise generator;

        public WorldGenerator()
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
