using OpenTK;
using VoxelRPG.Engine.Game.Components;

namespace VoxelRPG.Engine.Graphics.Meshes
{
    public abstract class Mesh
    {
        public Transform Transform;

        public int VertexCount;
        public int IndiceCount;
        public int ColorCount;

        public abstract Vector3[] GetVertices();
        public abstract int[] GetIndices(int offset = 0);
        public abstract Vector3[] GetColors();
        public abstract Matrix4 CalculateModelMatrix();
    }
}
