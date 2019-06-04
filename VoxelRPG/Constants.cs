using OpenTK;
using System.Collections.Generic;
using VoxelRPG.Game;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG
{
    public static class Constants
    {
        public static class Camera
        {
            public static readonly float FarClippingPane = 200;
            public static readonly float NearClippingPane = 1;
        }

        public static class World
        {
            public static class Chunk
            {
                public static readonly int Size = 64;
                public static readonly int Height = 128;
                public static class Colors
                {
                    public static Vector3 Grass = new Vector3(0.02f, 0.48f, 0.05f);
                    public static Vector3 Stone = new Vector3(0.57f, 0.57f, 0.57f);
                    public static Vector3 Snow = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }
        }

        public static class Enums
        {
            public enum ComponentType
            {
                Transform, Renderer, Rigidbody
            }
            public enum GameObjectType
            {
                ENVIROMENT, DEBUG
            }
        }
    }
}
