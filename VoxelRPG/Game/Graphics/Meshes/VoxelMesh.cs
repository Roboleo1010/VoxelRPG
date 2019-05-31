using OpenTK;
using System;
using System.Collections.Generic;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game.Enviroment;

namespace VoxelRPG.Game.Graphics.Meshes
{
    public class VoxelMesh : Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> colors = new List<Vector3>();
        public List<int> indices = new List<int>();

        Voxel[,,] voxels;
        bool ignoreNeighbourTransparency;

        public VoxelMesh() { }

        public VoxelMesh(Voxel[,,] v, bool ignoreNeighbourTransparency = false)
        {
            voxels = v;
            this.ignoreNeighbourTransparency = ignoreNeighbourTransparency;
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

        public override void Copy(Mesh mesh)
        {
            vertices.AddRange(mesh.GetVertices());
            colors.AddRange(mesh.GetColors());
            indices.AddRange(mesh.GetIndices());

            VertexCount = vertices.Count;
            IndiceCount = indices.Count;
            ColorCount = vertices.Count;
        }

        public void Build()
        {
            for (int x = 0; x < voxels.GetLength(0); x++)
                for (int y = 0; y < voxels.GetLength(1); y++)
                    for (int z = 0; z < voxels.GetLength(2); z++)
                        GetMeshData(x, y, z, voxels[x, y, z]);

            VertexCount = vertices.Count;
            IndiceCount = indices.Count;
            ColorCount = vertices.Count;
        }

        bool HasToRenderSide(int x, int y, int z)
        {
            if (x < 0 || x >= voxels.GetLength(0) ||
                y < 0 || y >= voxels.GetLength(1) ||
                z < 0 || z >= voxels.GetLength(2))
                return ignoreNeighbourTransparency;

            return voxels[x, y, z] == null;
        }

        private void GetMeshData(int x, int y, int z, Voxel voxel)
        {
            bool renderFront = HasToRenderSide(x - 1, y, z);
            bool renderBack = HasToRenderSide(x + 1, y, z);
            bool renderLeft = HasToRenderSide(x, y, z - 1);
            bool renderRight = HasToRenderSide(x, y, z + 1);
            bool renderTop = HasToRenderSide(x, y + 1, z);
            bool renderBottom = HasToRenderSide(x, y - 1, z);

            if (voxel != null && (renderFront == true || renderBack == true || renderLeft == true ||
                                          renderRight == true || renderTop == true || renderBottom == true))
            {
                int vOffset = vertices.Count;

                //Bottom
                vertices.Add(new Vector3(0f + x, 0f + y, 0f + z));
                vertices.Add(new Vector3(1f + x, 0f + y, 0f + z));
                vertices.Add(new Vector3(1f + x, 1f + y, 0f + z));
                vertices.Add(new Vector3(0f + x, 1f + y, 0f + z));

                //Top                                 
                vertices.Add(new Vector3(0f + x, 0f + y, 1f + z));
                vertices.Add(new Vector3(1f + x, 0f + y, 1f + z));
                vertices.Add(new Vector3(1f + x, 1f + y, 1f + z));
                vertices.Add(new Vector3(0f + x, 1f + y, 1f + z));

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