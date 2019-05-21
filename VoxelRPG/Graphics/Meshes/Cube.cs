using OpenTK;

namespace VoxelRPG.Graphics.Meshes
{
    public class Cube : Mesh
    {
        public Cube()
        {
            VertexCount = 8;
            IndiceCount = 36;
            ColorCount = 8;
        }

        public override Vector3[] GetVertices()
        {
            return new Vector3[] {
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(1f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(1f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(0f, 1f, 1f),
            }; ;
        }

        public override int[] GetIndices()
        {
            return new int[]{
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
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) *
                          Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
