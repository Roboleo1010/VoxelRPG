using OpenTK;

namespace VoxelRPG.Engine.Game.Components
{
    public class Rigidbody : Component
    {
        Vector3 velocity = Vector3.Zero;

        public void AddForce(Vector3 f)
        {
            velocity = velocity + f;
        }

        public void SetForce(Vector3 f)
        {
            velocity = f;
        }

        public override void OnUpdate(float deltaTime)
        {
            //AddForce(new Vector3(0, -0.005f, 0));

            Parent.Transform.Position = new Vector3(Parent.Transform.Position.X + velocity.X * deltaTime,
                                                    Parent.Transform.Position.Y + velocity.Y * deltaTime,
                                                    Parent.Transform.Position.Z + velocity.Z * deltaTime);
        }
    }
}
