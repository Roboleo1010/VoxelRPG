
using VoxelRPG.Engine.Graphics.Meshes;

namespace VoxelRPG.Engine.Game
{
    public class MeshGameObject : GameObject
    {
        Mesh mesh;
        public MeshGameObject(Mesh m)
        {
            mesh = m;
        }

        protected override Mesh GetMeshVirtual()
        {
            return mesh;
        }
    }
}
