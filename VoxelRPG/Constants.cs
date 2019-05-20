using VoxelRPG.Game;

namespace VoxelRPG
{
    public static class Constants
    {
        public static GameManager gameManager;


        public static class Camera
        {
            public static readonly float FarClippingPane = 200;
            public static readonly float NearClippingPane = 1;
        }

        public static class World
        {
            public static class Chunk
            {
                public static readonly int Size = 4;
                public static readonly int Height = 4;
            }
        }
    }
}
