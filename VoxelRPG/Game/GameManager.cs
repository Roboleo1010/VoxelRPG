using System;
using VoxelRPG.Engine.Graphics;
using VoxelRPG.Game.Entity;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Input;

namespace VoxelRPG.Game
{
    public static class GameManager
    {
        public static Window window;
        public static InputManager inputManager;
        public static World world;
        public static Player player;
        public static Random Random = new Random();

        public static float Time = 0;
    }
}
