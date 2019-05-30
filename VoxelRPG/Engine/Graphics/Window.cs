using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Graphics.Rendering;
using VoxelRPG.Game;
using VoxelRPG.Game.Entity;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Input;

namespace VoxelRPG.Engine.Graphics
{
    public class Window : GameWindow
    {
        Player player;
        List<GameObject> gameObjects = new List<GameObject>();

        RenderBuffer meshRendering;

        public Window() : base(1024, 724, new GraphicsMode(32, 24, 0, 4))
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitGame();
            InitGraphics();

            gameObjects.Add(GameObjectFactory.Cube(Vector3.Zero, Vector3.Zero, new Vector3(1, 5, 10)));

            meshRendering = new RenderBuffer(gameObjects);
        }

        //Update physics
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GameManager.Time += (float)e.Time;
            GameManager.inputManager.ProcessInput(Focused);

            Title = RenderFrequency + "";

            float deltaTime = GameManager.Time - (float)e.Time;
            foreach (GameObject g in gameObjects)
                g.OnUpdate(deltaTime);
        }

        //Update Rendering
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            meshRendering.GatherData();
            meshRendering.BindBuffers();
            meshRendering.Render();

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
            player = new Player();
            player.Transform.Position = new Vector3(0, 25, 0);
            GameManager.player = player;

            GameManager.inputManager = new InputManager(this, player);

            GameManager.world.Init();
        }

        void InitGraphics()
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(8f);
            CursorVisible = false;
        }

        #region GameObject Helper
        public void AddGameObject(GameObject o)
        {
            gameObjects.Add(o);
        }

        public void RemoveGameObject(GameObject o)
        {
            gameObjects.Remove(o);
        }
        #endregion
    }
}