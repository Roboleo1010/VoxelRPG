namespace VoxelRPG.Engine.Game.Components
{
    public abstract class Component
    {
        public GameObject Parent;

        public virtual void OnUpdate(float deltaTime)
        {

        }
    }
}
