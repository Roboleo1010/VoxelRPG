using OpenTK;

namespace VoxelRPG.Enige.Game
{
    public abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public virtual void OnUpdate(float deltaTime)
        {

        }
    }
}
