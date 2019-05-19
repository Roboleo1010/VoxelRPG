using OpenTK;
using OpenTK.Input;
using VoxelRPG.Graphics;

namespace VoxelRPG.Input
{
    public class InputManager
    {
        Window window;
        Camera camera;

        public Vector2 lastMousePos = new Vector2();

        public InputManager(Window window, Camera camera)
        {
            this.window = window;
            this.camera = camera;
        }

        public void ProcessInput(bool focused)
        {
            if (focused)
            {
                Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                camera.AddRotation(delta.X, delta.Y);
                lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                if (Keyboard.GetState().IsKeyDown(Key.W))
                {
                    camera.Move(0f, 0.1f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.S))
                {
                    camera.Move(0f, -0.1f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.A))
                {
                    camera.Move(-0.1f, 0f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.D))
                {
                    camera.Move(0.1f, 0f, 0f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.Q) || Keyboard.GetState().IsKeyDown(Key.Space))
                {
                    camera.Move(0f, 0f, 0.1f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.E) || Keyboard.GetState().IsKeyDown(Key.ControlLeft))
                {
                    camera.Move(0f, 0f, -0.1f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.Escape))
                {
                    window.Exit();
                }
            }
        }
    }
}
