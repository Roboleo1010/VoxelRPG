using OpenTK;
using OpenTK.Input;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Graphics;
using VoxelRPG.Game;
using VoxelRPG.Game.Entity;

namespace VoxelRPG.Input
{
    public class InputManager
    {
        Window window;
        Player player;

        public Vector2 lastMousePos = new Vector2();

        public InputManager(Window window, Player p)
        {
            this.window = window;
            this.player = p;
        }

        public void ProcessInput(bool focused)
        {
            if (focused)
            {
                Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                player.AddRotation(delta.X, delta.Y);
                lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                if (Keyboard.GetState().IsKeyDown(Key.W))
                {
                    player.Move(0f, 0.1f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.S))
                {
                    player.Move(0f, -0.1f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.A))
                {
                    player.Move(-0.1f, 0f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.D))
                {
                    player.Move(0.1f, 0f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.Q) || Keyboard.GetState().IsKeyDown(Key.Space))
                {
                    player.Move(0f, 0f, 0.1f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.E) || Keyboard.GetState().IsKeyDown(Key.ControlLeft))
                {
                    player.Move(0f, 0f, -0.1f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.Escape))
                {
                    Debug.CSV.End("fps");
                    window.Exit();
                }
            }
        }
    }
}
