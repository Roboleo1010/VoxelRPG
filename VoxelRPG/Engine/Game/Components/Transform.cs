using System;
using OpenTK;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Game.Components
{
    public class Transform : Component
    {
        Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                RoundedPosition = new Vector3Int((int)position.X, (int)position.Y, (int)position.Z);
            }
        }

        public Vector3Int RoundedPosition { get; set; }
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
    }
}
