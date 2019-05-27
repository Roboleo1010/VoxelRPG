using OpenTK;
using System;
using System.Collections.Generic;
using VoxelRPG.Game.Generation;

namespace VoxelRPG.Graphics.Meshes
{
    public class Chunk : Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

        Vector3 chunkPosInList;
        Vector3 chunkPosInWorld;

        BlockType[,,] blockTypes = new BlockType[Constants.World.Chunk.Size, Constants.World.Chunk.Height, Constants.World.Chunk.Size];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        Random r;

        WorldGenerator generator;

        enum BlockType
        {
            STONE, GRASS, AIR
        };

        public Chunk(int x, int z)
        {
            generator = new WorldGenerator();
            r = new Random(x + z);

            chunkPosInList = new Vector3(x, 0, z);
            chunkPosInWorld = new Vector3(x * Constants.World.Chunk.Size, 0, z * Constants.World.Chunk.Size);

            GatherData();
            Build();

            VertCount = vertices.Count;
            IndiceCount = indices.Count;
            ColorDataCount = vertices.Count;
        }

        public override Vector3[] GetVerts()
        {
            return vertices.ToArray();
        }

        public override int[] GetIndices(int offset = 0)
        {
            int[] inds = indices.ToArray();

            if (offset != 0)
                for (int i = 0; i < inds.Length; i++)
                    inds[i] += offset;

            return inds;
        }

        public override Vector3[] GetColorData()
        {
            return colors.ToArray();
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public void GatherData()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    heightData[x, z] = generator.GetHeight(x + (int)chunkPosInWorld.X, z + (int)chunkPosInWorld.Z);
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        if (y > heightData[x, z])
                            blockTypes[x, y, z] = BlockType.AIR;
                        else if (y == heightData[x, z])
                            blockTypes[x, y, z] = BlockType.GRASS;
                        else if (y < heightData[x, z])
                            blockTypes[x, y, z] = BlockType.STONE;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        GetMeshData(x, y, z, blockTypes[x, y, z]);
                    }
        }

        bool HasToRenderSide(int x, int y, int z)
        {
            if (x < 0 || x >= Constants.World.Chunk.Size ||
                y < 0 || y >= Constants.World.Chunk.Height ||
                z < 0 || z >= Constants.World.Chunk.Size)
                return false;

            return blockTypes[x, y, z] == BlockType.AIR;
        }

        private void GetMeshData(int x, int y, int z, BlockType type)
        {
            Vector3 actualVoxelPosition = new Vector3(chunkPosInWorld.X + x, y, chunkPosInWorld.Z + z);

            Vector3 color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

            bool renderFront = HasToRenderSide(x - 1, y, z);
            bool renderBack = HasToRenderSide(x + 1, y, z);
            bool renderLeft = HasToRenderSide(x, y, z - 1);
            bool renderRight = HasToRenderSide(x, y, z + 1);
            bool renderTop = HasToRenderSide(x, y + 1, z);
            bool renderBottom = HasToRenderSide(x, y - 1, z);

            if (type != BlockType.AIR && renderFront == true || renderBack == true || renderLeft == true ||
                                         renderRight == true || renderTop == true || renderBottom == true)
            {
                int vOffset = vertices.Count;

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