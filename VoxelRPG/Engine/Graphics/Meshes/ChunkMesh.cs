using OpenTK;
using System;
using System.Collections.Generic;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Graphics.Meshes
{
    public class ChunkMesh : Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

        Chunk chunk;
        Random random;

        public ChunkMesh(Chunk c)
        {
            chunk = c;
            random = new Random();
        }

        public override Vector3[] GetVertices()
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

        public override Vector3[] GetColors()
        {
            return colors.ToArray();
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        GetMeshData(x, y, z, chunk.voxels[x, y, z].Type);
                    }

            VertexCount = vertices.Count;
            IndiceCount = indices.Count;
            ColorCount = vertices.Count;
        }

        bool HasToRenderSide(int x, int y, int z)
        {
            if (x < 0 || x >= Constants.World.Chunk.Size ||
                y < 0 || y >= Constants.World.Chunk.Size ||
                z < 0 || z >= Constants.World.Chunk.Size)
                return false;

            return chunk.voxels[x, y, z].IsTransparent;
        }

        private void GetMeshData(int x, int y, int z, BlockType type)
        {
            Vector3Int actualVoxelPosition = new Vector3Int(chunk.chunkWorldPos.X + x,
                                                            chunk.chunkWorldPos.Y + y,
                                                            chunk.chunkWorldPos.Z + z);

            Vector3 color;
            if (type == BlockType.GRASS)
                color = Constants.World.Chunk.Colors.Grass;
            else if(type == BlockType.STONE)
                color = Constants.World.Chunk.Colors.Stone;
            else
                color = Constants.World.Chunk.Colors.Snow;

            color = new Vector3((float)(color.X + random.NextDouble() * 0.08f), (float)(color.Y + random.NextDouble() * 0.08f), (float)(color.Z + random.NextDouble() * 0.08f));

            bool renderFront = HasToRenderSide(x - 1, y, z);
            bool renderBack = HasToRenderSide(x + 1, y, z);
            bool renderLeft = HasToRenderSide(x, y, z - 1);
            bool renderRight = HasToRenderSide(x, y, z + 1);
            bool renderTop = HasToRenderSide(x, y + 1, z);
            bool renderBottom = HasToRenderSide(x, y - 1, z);

            if (type != BlockType.AIR && (renderFront == true || renderBack == true || renderLeft == true ||
                                          renderRight == true || renderTop == true || renderBottom == true))
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