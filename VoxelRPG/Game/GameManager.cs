using OpenTK;
using System;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Graphics;
using VoxelRPG.Engine.Manager.Models;
using VoxelRPG.Game.Entity;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Game.Generation;
using VoxelRPG.Input;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game
{
    public static class GameManager
    {
        public static Window Window;
        public static InputManager InputManager;
        public static World World;
        public static Player Player;

        public static int Seed;
        public static float Time = 0;

        public static void Start(Window window)
        {
            Window = window;
            Debug.Init();
            Debug.CSV.Start("fps", new string[] { "Time", "FPS" });

            ModelManager.Start();

            Seed = new Random().Next(int.MinValue, int.MaxValue);
            WorldGenerator generator = new WorldGenerator(new Vector3Int(0, 0, 0));
            Player = new Player(new Vector3(0, generator.GetHeight(0, 0) + 3, 0));

            World = new World();
            World.Start();            

            InputManager = new InputManager(Window, Player);
        }
    }
}
