using OpenTK;
using System;
using VoxelRPG.Engine;
using VoxelRPG.Engine.Graphics;

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
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Rotation.X), 0, (float)Math.Cos((float)Rotation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, moveSpeed);

            Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * mouseSensitivity;
            y = y * mouseSensitivity;

            Rotation = new Vector3((Rotation.X + x) % ((float)Math.PI * 2.0f),
                                   Math.Max(Math.Min(Rotation.Y + y, (float)Math.PI / 2.0f - 0.1f),
                                   (float)-Math.PI / 2.0f + 0.1f), 0);
        }
    }
}