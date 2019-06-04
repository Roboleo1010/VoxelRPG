using OpenTK;
using System;
using VoxelRPG.Engine.Graphics.Meshes;

namespace VoxelRPG.Game.Graphics.Meshes
{
    class CubeMesh : Mesh
    {
        public CubeMesh()
        {
            VertexCount = 8;
            ColorCount = 8;
            NormalCount = 8;
            IndiceCount = 36;
        }

        public override Vector3[] GetVertices()
        {
            return new Vector3[] {
                new Vector3(0, 0,  0),
                new Vector3(1, 0,  0),
                new Vector3(1, 1,  0),
                new Vector3(0, 1,  0),
                new Vector3(0, 0,  1),
                new Vector3(1, 0,  1),
                new Vector3(1, 1,  1),
                new Vector3(0, 1,  1),
            };
        }

        public override Vector3[] GetColors()
        {
            return new Vector3[] {
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f)
            };
        }

        public override Vector3[] GetNormals()
        {
            return new Vector3[] {
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY
            };
        }

        public override int[] GetIndices(int offset = 0)
        {
            int[] inds = new int[] {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            if (offset != 0)
                for (int i = 0; i < inds.Length; i++)
                    inds[i] += offset;

            return inds;
        }

        public override Matrix4 CalculateModelMatrix()
        {
            return Matrix4.CreateScale(Transform.Scale) * Matrix4.CreateRotationX(Transform.Rotation.X) * Matrix4.CreateRotationY(Transform.Rotation.Y) * Matrix4.CreateRotationZ(Transform.Rotation.Z) * Matrix4.CreateTranslation(Transform.Position);
        }

        public override void Copy(Mesh mesh)
        {
            throw new NotImplementedException();
        }
    }
}