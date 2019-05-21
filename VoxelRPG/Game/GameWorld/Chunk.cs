using OpenTK;
using System;
using System.Collections.Generic;
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

        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

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
                        blockTypes[x, z, y] = BlockType.GRASS;

                        //if (y > heightData[x, z])
                        //    blockTypes[x, z, y] = BlockType.AIR;
                        //else if (y == heightData[x, z])
                        //    blockTypes[x, z, y] = BlockType.GRASS;
                        //else if (y < heightData[x, z])
                        //    blockTypes[x, z, y] = BlockType.STONE;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        GetMeshData(x, z, y, blockTypes[x, z, y]);
                    }
        }

        public void Draw()
        {
            GameManager.window.meshes.Add(new GenericMesh(vertices.ToArray(), colors.ToArray(), indices.ToArray()));
        }

        bool HasToRenderSide(int x, int z, int y)
        {
            if (x < 0 || x >= Constants.World.Chunk.Size ||
                z < 0 || z >= Constants.World.Chunk.Size ||
                y < 0 || y >= Constants.World.Chunk.Height)
                return false;

            return blockTypes[x, z, y] == BlockType.AIR;
        }

        private void GetMeshData(int x, int z, int y, BlockType type)
        {
            Vector3Int actualVoxelPosition = new Vector3Int(Position.X + x, Position.Z + z, Position.Z + y);

            Vector3 color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

            bool renderFront =  true; //HasToRenderSide(x - 1, z, y);        //front
            bool renderBack =   true; //HasToRenderSide(x + 1, z, y);        //back
            bool renderLeft =   true; //HasToRenderSide(x, z - 1, y);        //left
            bool renderRight =  true; //HasToRenderSide(x, z + 1, y);        //right
            bool renderTop =    true; //HasToRenderSide(x, z, y + 1);        //top
            bool renderBottom = true; //HasToRenderSide(x, z, y - 1);        //bottom


            if (type != BlockType.AIR && (renderFront == true || renderBack == true || renderLeft == true ||
                renderRight == true || renderTop == true || renderBottom == true))
            {
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));

                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));

                for (int i = 0; i < 8; i++)
                    colors.Add(color);

                int vOffset = vertices.Count;

                if (renderFront)
                    indices.AddRange(new int[] { vOffset + 0, vOffset + 7, vOffset + 3, vOffset + 0, vOffset + 4, vOffset + 7 }); //front
                if (renderBack)
                    indices.AddRange(new int[] { vOffset + 1, vOffset + 2, vOffset + 6, vOffset + 6, vOffset + 5, vOffset + 1 }); //back
                if (renderLeft)
                    indices.AddRange(new int[] { vOffset + 0, vOffset + 2, vOffset + 1, vOffset + 0, vOffset + 3, vOffset + 2 });  //left
                if (renderRight)
                    indices.AddRange(new int[] { vOffset + 4, vOffset + 5, vOffset + 6, vOffset + 6, vOffset + 7, vOffset + 4 }); //right
                if (renderTop)
                    indices.AddRange(new int[] { vOffset + 2, vOffset + 3, vOffset + 6, vOffset + 6, vOffset + 3, vOffset + 7 }); //top
                if (renderBottom)
                    indices.AddRange(new int[] { vOffset + 0, vOffset + 1, vOffset + 5, vOffset + 0, vOffset + 5, vOffset + 4 }); //bottom
            }
        }
    }
}