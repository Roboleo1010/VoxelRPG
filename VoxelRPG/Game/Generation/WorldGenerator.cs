using VoxelRPG.Game.Enviroment;
using VoxelRPG.Libs.Noise;
using VoxelRPG.Utilitys;

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

        public Voxel GetVoxel(Vector3Int posInWorld, Vector3Int posInChunk, int height)
        {
            if (posInChunk.Y > height)
                return null;
            else if (posInChunk.Y > 20)
                return new Voxel(posInChunk, Constants.World.Chunk.Colors.Snow);
            else if (posInChunk.Y > 5)
                return new Voxel(posInChunk, Constants.World.Chunk.Colors.Grass);
            else
                return new Voxel(posInChunk, Constants.World.Chunk.Colors.Stone);
        }
    }
}
