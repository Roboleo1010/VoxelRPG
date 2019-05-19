using OpenTK;
using System.Collections.Generic;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Graphics.Volumes
{
    public class AdaptiveCube : Volume
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> colors = new List<Vector3>();
        List<int> indices = new List<int>();

        public AdaptiveCube(Vector3 color, Vector3Int pos, bool hasFrontNeighbour, bool hasBackNeighbour, bool hasLeftNeighbour,
                                                           bool hasRightNeighbour, bool hasTopNeighbour, bool hasBottomNeighbour)
        {
            IndiceCount = 36;

            vertices.Add(new Vector3(0f + pos.X, 0f + pos.Y, 0f + pos.Z));
            vertices.Add(new Vector3(1f + pos.X, 0f + pos.Y, 0f + pos.Z));
            vertices.Add(new Vector3(1f + pos.X, 1f + pos.Y, 0f + pos.Z));
            vertices.Add(new Vector3(0f + pos.X, 1f + pos.Y, 0f + pos.Z));
            vertices.Add(new Vector3(0f + pos.X, 0f + pos.Y, 1f + pos.Z));
            vertices.Add(new Vector3(1f + pos.X, 0f + pos.Y, 1f + pos.Z));
            vertices.Add(new Vector3(1f + pos.X, 1f + pos.Y, 1f + pos.Z));
            vertices.Add(new Vector3(0f + pos.X, 1f + pos.Y, 1f + pos.Z));

            VertexCount = vertices.Count;

            for (int i = 0; i < VertexCount; i++)
                colors.Add(color);

            ColorDataCount = colors.Count;
        }

        public override Vector3[] GetVertices()
        {
            return vertices.ToArray();
        }

        public override int[] GetIndices(int offset = 0)
        {
            int[] indices = new int[]{
                //front
                0, 7, 3,
                0, 4, 7,
                //back
                1, 2, 6,
                6, 5, 1,
                //left
                0, 2, 1,
                0, 3, 2,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            if (offset != 0)
                for (int i = 0; i < indices.Length; i++)
                    indices[i] += offset;

            return indices;
        }

        public override Vector3[] GetColors()
        {
            return colors.ToArray();
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;

            //ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) *
            //              Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
