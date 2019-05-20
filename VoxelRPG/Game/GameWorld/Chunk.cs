using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using VoxelRPG.Game.Generation;
using VoxelRPG.Graphics.Meshes;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.GameWorld
{
    public class Chunk
    {
        Random r;

        //Chunk Data
        public Vector2Int Position; // in world
        bool[,,] isSolid = new bool[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Height];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        //Mesh Data
        List<Mesh> meshes = new List<Mesh>();

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
                        isSolid[x, z, y] = true;

                        //if (y > 2)//heightData[x, z])
                        //    isSolid[x, z, y] = false;
                        //else
                        //    isSolid[x, z, y] = true;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        meshes.Add(GetMeshData(x, z, y));
                    }
        }

        public void Draw()
        {
            Constants.gameManager.window.meshes.AddRange(meshes);
        }

        bool IsSolid(int x, int z, int y)
        {
            if (x < 0 || x >= Constants.World.Chunk.Size ||
                z < 0 || z >= Constants.World.Chunk.Size ||
                y < 0 || y >= Constants.World.Chunk.Height)
                return false;

            return isSolid[x, z, y];
        }

        private Mesh GetMeshData(int x, int z, int y)
        {
            return new AdaptiveCube(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), new Vector3Int(x, z, y),
                IsSolid(x - 1, z, y),       //front
                IsSolid(x + 1, z, y),       //back
                IsSolid(x, z - 1, y),       //left
                IsSolid(x, z + 1, y),       //right
                IsSolid(x, z, y + 1),       //top
                IsSolid(x, z, y - 1));      //bottom
        }
    }
}