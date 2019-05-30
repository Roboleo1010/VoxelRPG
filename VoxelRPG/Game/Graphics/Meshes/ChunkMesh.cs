using OpenTK;
using System.Collections.Generic;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Graphics.Meshes
{
    public class ChunkMesh : Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

        Chunk chunk;

        public ChunkMesh(Chunk c)
        {
            chunk = c;
            Transform = chunk.Transform;
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

        public override Matrix4 CalculateModelMatrix()
        {
            return Matrix4.CreateScale(Transform.Scale) * Matrix4.CreateRotationX(Transform.Rotation.X) * Matrix4.CreateRotationY(Transform.Rotation.Y) * Matrix4.CreateRotationZ(Transform.Rotation.Z) * Matrix4.CreateTranslation(Transform.Position);
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        GetMeshData(x, y, z, chunk.Voxels[x, y, z]);
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

            return chunk.Voxels[x, y, z].IsTransparent;
        }

        private void GetMeshData(int x, int y, int z, Voxel voxel)
        {
            Vector3Int actualVoxelPosition = new Vector3Int(chunk.ChunkWorldPosition.X + x,
                                                            chunk.ChunkWorldPosition.Y + y,
                                                            chunk.ChunkWorldPosition.Z + z);

            bool renderFront = HasToRenderSide(x - 1, y, z);
            bool renderBack = HasToRenderSide(x + 1, y, z);
            bool renderLeft = HasToRenderSide(x, y, z - 1);
            bool renderRight = HasToRenderSide(x, y, z + 1);
            bool renderTop = HasToRenderSide(x, y + 1, z);
            bool renderBottom = HasToRenderSide(x, y - 1, z);

            if (voxel.Type != BlockType.AIR && (renderFront == true || renderBack == true || renderLeft == true ||
                                          renderRight == true || renderTop == true || renderBottom == true))
            {
                chunk.IsEmpty = false;

                int vOffset = vertices.Count;

                //Bottom
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 0f + actualVoxelPosition.Z));

                //Top
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 0f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(1f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));
                vertices.Add(new Vector3(0f + actualVoxelPosition.X, 1f + actualVoxelPosition.Y, 1f + actualVoxelPosition.Z));

                for (int i = 0; i < 8; i++)
                    colors.Add(voxel.Color);

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