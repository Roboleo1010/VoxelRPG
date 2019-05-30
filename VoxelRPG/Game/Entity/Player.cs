using OpenTK;
using System;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics;
using VoxelRPG.Game.Enviroment;

namespace VoxelRPG.Game.Entity
{
    public class Player : Entity
    {
        public Camera camera;

        float moveSpeed = 0.2f;
        float mouseSensitivity = 0.0025f;

        public Player()
        {
            camera = new Camera(this);
            // Rigidbody.SetForce(new Vector3(0, 1, 0));
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(Transform.Rotation.X), 0, (float)Math.Cos(Transform.Rotation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, moveSpeed);

            Transform.Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * mouseSensitivity;
            y = y * mouseSensitivity;

            Transform.Rotation = new Vector3((Transform.Rotation.X + x) % ((float)Math.PI * 2.0f),
                                   Math.Max(Math.Min(Transform.Rotation.Y + y, (float)Math.PI / 2.0f - 0.1f),
                                   (float)-Math.PI / 2.0f + 0.1f), 0);
        }
    }
}