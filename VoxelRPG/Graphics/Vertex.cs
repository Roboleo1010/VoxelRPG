using OpenTK;

namespace VoxelRPG.Graphics
{
    struct Vertex
    {
        public const int Size = (3 + 4) * 4; // size of struct in bytes

        private readonly Vector3 position;
        private readonly Vector4 color;

        public Vertex(Vector3 position, Vector4 color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
