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

        enum BlockType
        {
            STONE, GRASS, AIR
        };

        //Chunk Data
        public Vector2Int Position; // in world
        BlockType[,,] blockTypes = new BlockType[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Height];
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
                        if (y > heightData[x, z])
                            blockTypes[x, z, y] = BlockType.AIR;
                        else if (y == heightData[x, z])
                            blockTypes[x, z, y] = BlockType.GRASS;
                        else if (y < heightData[x, z])
                            blockTypes[x, z, y] = BlockType.STONE;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        Mesh m = GetMeshData(x, z, y, blockTypes[x, z, y]);
                        if (m != null)
                            meshes.Add(m);
                    }
        }

        public void Draw()
        {
            MeshCollection meshCollection = new MeshCollection(meshes);
            Constants.gameManager.window.meshes.Add(meshCollection);
        }

        bool HasToRenderSide(int x, int z, int y)
        {
            if (x < 0 || x >= Constants.World.Chunk.Size ||
                z < 0 || z >= Constants.World.Chunk.Size ||
                y < 0 || y >= Constants.World.Chunk.Height)
                return false;

            return blockTypes[x, z, y] == BlockType.AIR;
        }

        private Mesh GetMeshData(int x, int z, int y, BlockType type)
        {
            Vector3 color;

            //if (type == BlockType.GRASS)
            // color = new Vector3((float)21 / 255, (float)188 / 255, (float)18 / 255);
            //else
            color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

            if (type != BlockType.AIR)
            {
                return new AdaptiveCube(color, new Vector3Int(x, z, y),
                    HasToRenderSide(x - 1, z, y),       //front
                    HasToRenderSide(x + 1, z, y),       //back
                    HasToRenderSide(x, z - 1, y),       //left
                    HasToRenderSide(x, z + 1, y),       //right
                    HasToRenderSide(x, z, y + 1),       //top
                    HasToRenderSide(x, z, y - 1));      //bottom
            }

            return null;
        }
    }
}