using OpenTK;

namespace VoxelRPG.Engine.Graphics.Meshes
{
    public abstract class Mesh
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public int VertexCount;
        public int IndiceCount;
        public int ColorCount;
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public abstract Vector3[] GetVertices();
        public abstract int[] GetIndices(int offset = 0);
        public abstract Vector3[] GetColors();
        public abstract void CalculateModelMatrix();
    }
}
