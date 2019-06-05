using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoxelRPG.Engine.Manager.Models;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Libs.Noise;
using VoxelRPG.Utilitys;
using Debug = VoxelRPG.Engine.Diagnosatics.Debug;

namespace VoxelRPG.Game.Generation
{
    public class WorldGenerator
    {
        Random random;
        FastNoise terrainNoise;
        FastNoise colorNoise;

        Vector3Int chunkPosition;
        Voxel[,,] voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Height, Constants.World.Chunk.Size];
        int[,] height = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        public WorldGenerator(Vector3Int pos)
        {
            chunkPosition = pos;

            random = new Random(GameManager.Seed);
            terrainNoise = new FastNoise(GameManager.Seed);
            terrainNoise.SetFrequency(0.005f); // größer = steiler, kleiner = flacher

            colorNoise = new FastNoise(-GameManager.Seed);
            colorNoise.SetFrequency(0.05f); // größer = steiler, kleiner = flacher
        }

        public void Generate()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            GetHeightData();
            GetGroundVoxels();
            GetGridModels();

            sw.Stop();
            Debug.LogInfo("Generated in " + sw.ElapsedMilliseconds);
        }

        public Voxel[,,] GetVoxels()
        {
            return voxels;
        }

        public Voxel GetVoxel(Vector3Int pos)
        {
            return voxels[pos.X, pos.Y, pos.Z];
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            return voxels[x, y, z];
        }

        void GetHeightData()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    height[x, z] = GetHeight(x + chunkPosition.X, z + chunkPosition.Z);
        }

        public int GetHeight(int x, int z)
        {
            return (int)MathUtility.Map(0, Constants.World.Chunk.Height - 40, -1, 1, FBM(x, z, 5, 0.5f));
        }

        float FBM(float x, float z, int oct, float pers)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            float maxValue = 0;
            for (int i = 0; i < oct; i++)
            {
                total += terrainNoise.GetPerlin(x * frequency, z * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= pers;
                frequency *= 2;
            }

            return total / maxValue;
        }

        void GetGroundVoxels()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    {
                        Vector3Int posInChunk = new Vector3Int(x, y, z);
                        Vector3Int posInWorld = new Vector3Int(chunkPosition.X + x, chunkPosition.Y + y, chunkPosition.Z + z);
                        if (posInWorld.Y <= height[x, z])
                            voxels[posInChunk.X, posInChunk.Y, posInChunk.Z] = new Voxel(posInWorld, GetGroundColor(posInWorld, posInChunk));
                    }
        }

        Vector3 GetGroundColor(Vector3Int posInWorld, Vector3Int posInChunk)
        {
            Vector3 color;

            if (posInWorld.Y == height[posInChunk.X, posInChunk.Z])
            {
                if (posInWorld.Y > 165)
                    color = Constants.World.Chunk.Colors.Snow;
                else
                    color = Constants.World.Chunk.Colors.Grass;
            }
            else if (posInWorld.Y > 40)
                color = Constants.World.Chunk.Colors.Dirt;
            else
                color = Constants.World.Chunk.Colors.Stone;

            return GetGradiend(color, posInWorld);
        }

        void GetGridModels()
        {
            Model model = ModelManager.GetModel("Tulip");

            float radius = Math.Max(model.gridSize.X, model.gridSize.Z) * 5;
            Vector2 regionSize = new Vector2(Constants.World.Chunk.Size, Constants.World.Chunk.Size);
            int rejectionSamples = 5;

            List<Vector2> points = PoissonDiscSampling.GeneratePoints(radius, regionSize, random, rejectionSamples);

            foreach (Vector2 point in points)
                foreach (ModelVoxel v in model.modelVoxels)
                {
                    Vector3Int pos = new Vector3Int((int)point.X + v.position.X, height[(int)point.X, (int)point.Y] + v.position.Y, (int)point.Y + v.position.Z);
                    if (pos.X >= 0 && pos.Y >= 0 && pos.Z >= 0 && pos.X < Constants.World.Chunk.Size && pos.Y < Constants.World.Chunk.Height && pos.Z < Constants.World.Chunk.Size)
                        voxels[pos.X, pos.Y, pos.Z] = new Voxel(pos, v.color);
                }
        }

        Vector3 GetGradiend(Vector3 color, Vector3Int position, float amount = 0.2f)
        {
            float gradient = MathUtility.Map(-amount, amount, -1, 1, terrainNoise.GetPerlin(position.X, position.Z));

            return new Vector3(color.X + gradient, color.Y + gradient, color.Z + gradient);
        }
    }
}
