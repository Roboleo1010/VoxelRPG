using OpenTK;
using System;
using System.Collections.Generic;

namespace VoxelRPG.Graphics.Meshes
{
    public class Chunk : Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

        Vector3 chunkPos;
        Vector3 chunkPosInWorld;

        BlockType[,,] blockTypes = new BlockType[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Height];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        Random r;

        enum BlockType
        {
            STONE, GRASS, AIR
        };

        public Chunk(int x, int z)
        {
            r = new Random(x+z);

            chunkPos = new Vector3(x, 0, z);
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
                    //heightData[x, z] = generator.GetHeight(x + chunkPos.X, z + chunkPos.Z);
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

        private void GetMeshData(int x, int z, int y, BlockType type)
        {
            Vector3 actualVoxelPosition = new Vector3(chunkPosInWorld.X + x, y, chunkPosInWorld.Z + z);

            Vector3 color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

            bool renderFront = true; //HasToRenderSide(x - 1, z, y);        //front
            bool renderBack = true; //HasToRenderSide(x + 1, z, y);        //back
            bool renderLeft = true; //HasToRenderSide(x, z - 1, y);        //left
            bool renderRight = true; //HasToRenderSide(x, z + 1, y);        //right
            bool renderTop = true; //HasToRenderSide(x, z, y + 1);        //top
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