
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

        public override Mesh GetMesh()
        {
            return mesh;
        }
    }
}
