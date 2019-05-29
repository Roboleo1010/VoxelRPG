using OpenTK;
using System;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;

namespace VoxelRPG.Engine.Graphics
{
    public class Camera
    {
        GameObject parent;
        Transform transform;

        public Camera(GameObject p)
        {
            parent = p;
            transform = parent.Transform;
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3
            {
                X = (float)(Math.Sin(transform.Rotation.X) * Math.Cos(transform.Rotation.Y)),
                Y = (float)Math.Sin(transform.Rotation.Y),
                Z = (float)(Math.Cos(transform.Rotation.X) * Math.Cos(transform.Rotation.Y))
            };

            return Matrix4.LookAt(transform.Position, transform.Position + lookat, Vector3.UnitY);
        }
    }
}
