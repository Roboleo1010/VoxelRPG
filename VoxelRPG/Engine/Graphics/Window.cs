using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Graphics.Rendering;
using VoxelRPG.Engine.Manager.Models;
using VoxelRPG.Game;
using VoxelRPG.Game.Entity;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Input;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Graphics
{
    public class Window : GameWindow
    {
        ChunkRenderBuffer chunkBuffer;

        public Window() : base(1024, 724, new GraphicsMode(32, 24, 0, 4))
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitGraphics();
            chunkBuffer = new ChunkRenderBuffer();
            InitGame();

            Random r = new Random();

            for (int i = 0; i < 40; i++)
                AddGameObject(GameObjectFactory.Model(new Vector3((int)((r.NextDouble() - 0.5) * 80), 0, (int)((r.NextDouble() - 0.5) * 80)), Vector3.Zero, Vector3.One, "Tulip"));
        }

        //Update physics
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GameManager.Time += (float)e.Time;
            GameManager.inputManager.ProcessInput(Focused);
            Title = RenderFrequency + "";


            GameManager.world.GenerateAround(GameManager.player.Transform.Position);

            //Update all GameObjects
            foreach (GameObject g in chunkBuffer.GetGameObjects())
                g.OnUpdate(GameManager.Time - (float)e.Time);
        }

        //Update Rendering
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            chunkBuffer.GatherData();
            chunkBuffer.BindBuffers();
            chunkBuffer.Render();

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height,
                                 Constants.Camera.NearClippingPane, Constants.Camera.FarClippingPane);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            GameManager.inputManager.lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        void InitGame()
        {
            Debug.Init();

            GameManager.window = this;
            GameManager.world = new World();
            Player player = new Player();
            player.Transform.Position = new Vector3(0, 25, 0);
            GameManager.player = player;

            GameManager.inputManager = new InputManager(this, player);

            ModelManager.Init();

            GameManager.world.Init();
        }

        void InitGraphics()
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(8f);
            CursorVisible = false;
        }

        #region GameObject management
        public void AddGameObject(GameObject o)
        {
            switch (o.Type)
            {
                case GameObjectType.ENVIROMENT:
                    chunkBuffer.AddGameObject(o);
                    break;
                default:
                    Debug.LogError("Object type not known");
                    break;
            }
        }

        public void RemoveGameObject(GameObject o)
        {
            switch (o.Type)
            {
                case GameObjectType.ENVIROMENT:
                    chunkBuffer.RemoveGameObject(o);
                    break;
                default:
                    Debug.LogError("Object type not known");
                    break;
            }
        }
        #endregion
    }
}