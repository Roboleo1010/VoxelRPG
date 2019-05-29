using OpenTK;
using System;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics;

namespace VoxelRPG.Game.Entity
{
    public class Player : Entity
    {
        public Camera camera;

        float moveSpeed = 0.2f;
        float mouseSensitivity = 0.0025f;

        Transform transform;

        public Player()
        {
            camera = new Camera(this);
            transform = Transform;
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(transform.Rotation.X), 0, (float)Math.Cos(transform.Rotation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, moveSpeed);

            transform.Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * mouseSensitivity;
            y = y * mouseSensitivity;

            transform.Rotation = new Vector3((transform.Rotation.X + x) % ((float)Math.PI * 2.0f),
                                   Math.Max(Math.Min(transform.Rotation.Y + y, (float)Math.PI / 2.0f - 0.1f),
                                   (float)-Math.PI / 2.0f + 0.1f), 0);
        }
    }
}