using OpenTK;
using VoxelRPG.Engine.Graphics.Meshes;

namespace VoxelRPG.Engine.Game
{
    public abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public virtual void OnUpdate(float deltaTime)
        { }

        public virtual Mesh GetMesh()
        {
            return null;
        }
    }
}
