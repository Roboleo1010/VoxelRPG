using OpenTK;
using System;
using System.Collections.Generic;
using VoxelRPG.Engine.Game;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Libs.Noise;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Generation
{
    public class WorldGenerator
    {
        FastNoise generator;
        Random random;

        public WorldGenerator(int seed)
        {
            random = new Random();
            generator = new FastNoise(seed);
            generator.SetFrequency(0.02f); // größer = steiler, kleiner = flacher
        }

        public Voxel[,,] Generate(Vector3Int chunkPosition)
        {
            Voxel[,,] voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Height, Constants.World.Chunk.Size];

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    int height = GetHeight(x + chunkPosition.X, z + chunkPosition.Z);

                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                        voxels[x, y, z] = GetVoxel(new Vector3Int(chunkPosition.X + x,
                                                                  chunkPosition.Y + y,
                                                                  chunkPosition.Z + z),
                                                                  new Vector3Int(x, y, z),
                                                                  height);
                }

            return voxels;
        }

        public int GetHeight(int x, int z)
        {
            float height = MathUtility.Map(0, 30, -1, 1, generator.GetPerlin(x, z));
            return (int)height;
        }

        public void AddFlowers()
        {
            float radius = 10;
            Vector2 regionSize = new Vector2(32, 32);
            int rejectionSamples = 10;
            List<Vector2> points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);

            foreach (Vector2 point in points)
                GameManager.window.AddGameObject(GameObjectFactory.Model(new Vector3(point.X, 30, point.Y), Vector3.Zero, Vector3.One, "Tulip"));
        }

        public Voxel GetVoxel(Vector3Int posInWorld, Vector3Int posInChunk, int height)
        {
            if (posInChunk.Y > height)
                return null;

            Vector3 color;

            if (posInChunk.Y > 20)
                color = Constants.World.Chunk.Colors.Snow;
            else if (posInChunk.Y > 5)
                color = Constants.World.Chunk.Colors.Grass;
            else
                color = Constants.World.Chunk.Colors.Stone;

            return new Voxel(posInWorld, new Vector3((float)(color.X + random.NextDouble() * 0.08f), (float)(color.Y + random.NextDouble() * 0.08f), (float)(color.Z + random.NextDouble() * 0.08f)));
        }
    }
}
