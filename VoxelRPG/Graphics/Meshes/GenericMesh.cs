using OpenTK;

namespace VoxelRPG.Graphics.Meshes
{
    class GenericMesh : Mesh
    {
        Vector3[] vertices;
        Vector3[] colors;
        int[] indices;

        public GenericMesh(Vector3[] vertices, Vector3[] colors, int[] indices)
        {
            this.vertices = vertices;
            VertexCount = vertices.Length;

            this.colors = colors;
            ColorCount = colors.Length;

            this.indices = indices;
            IndiceCount = indices.Length;
        }

        public override Vector3[] GetVertices()
        {
            return vertices;
        }

        public override int[] GetIndices()
        {
            return indices;
        }

        public override Vector3[] GetColors()
        {
            return colors;
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) *
                          Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
