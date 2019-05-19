using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using VoxelRPG.Game.Generation;
using VoxelRPG.Graphics.Volumes;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.GameWorld
{
    public class Chunk
    {
        Random r;

        //Chunk Data
        public Vector2Int Position = new Vector2Int(); // in world
        bool[,,] isSolid = new bool[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Height];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        //Mesh Data
        List<Volume> volumes = new List<Volume>();

        public Chunk(Vector2Int pos)
        {
            Position = pos;

            r = new Random();
        }

        public void GatherData()
        {
            WorldGeneration generator = new WorldGeneration();

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    heightData[x, z] = generator.GetHeight(x + Position.X, z + Position.Z);
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        if (y > heightData[x, z])
                            isSolid[x, z, y] = false;
                        else
                            isSolid[x, z, y] = true;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        volumes.Add(GetMeshData(x, z, y));
                    }
        }

        public void Draw()
        {
            Constants.gameManager.window.volumes.AddRange(volumes);
        }

        bool IsSolid(int x, int z, int y)
        {
            return isSolid[x, z, y];
        }

        private Volume GetMeshData(int x, int z, int y)
        {
            return new AdaptiveCube(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), new Vector3Int(x, y, z), false, false, false, false, false, false);
        }
    }
}