using OpenTK;
using System;
using VoxelRPG.Enige.Game;
using VoxelRPG.Game.Enviroment;

namespace VoxelRPG.Engine.Graphics
{
    public class Camera
    {
        GameObject parent;
        
        public Camera(GameObject p)
        {
            parent = p;
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3
            {
                X = (float)(Math.Sin((float)parent.Rotation.X) * Math.Cos((float)parent.Rotation.Y)),
                Y = (float)Math.Sin((float)parent.Rotation.Y),
                Z = (float)(Math.Cos((float)parent.Rotation.X) * Math.Cos((float)parent.Rotation.Y))
            };

            return Matrix4.LookAt(parent.Position, parent.Position + lookat, Vector3.UnitY);
        }
    }
}
