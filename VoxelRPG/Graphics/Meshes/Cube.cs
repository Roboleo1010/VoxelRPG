using OpenTK;

namespace VoxelRPG.Graphics.Meshes
{
    public class Cube : Mesh
    {
        Vector3 Color = new Vector3(1, 1, 1);
        Vector3[] vertices = new Vector3[8];

        public Cube(Vector3 color)
        {
            VertexCount = 8;
            IndiceCount = 36;
            ColorDataCount = 8;
            Color = color;

            vertices = new Vector3[] {
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(1f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(1f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(0f, 1f, 1f),
            };

        }

        public override Vector3[] GetVertices()
        {
            return vertices;
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
            return new Vector3[] {
                new Vector3(1, 0, 1),
                new Vector3(0.5f, 0, 1),
                new Vector3(1, 0.5f, 1),
                new Vector3(1, 0, 0.5f),
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0.5f),
                new Vector3(1, 1, 1),
            };
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;

            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) *
                          Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
