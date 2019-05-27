using VoxelRPG.Game;

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
                public static readonly int Size = 32;
            }
        }

        public static class Enums
        {
            public static class Chunk
            {
                public enum BlockType
                {
                    STONE, GRASS, AIR
                };
            }
        }
    }
}
