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
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Graphics
{
    public class Window : GameWindow
    {
        ChunkRenderBuffer chunkBuffer;
        List<GameObject> gameObjects = new List<GameObject>();

        public Window() : base(1024, 724, new GraphicsMode(32, 24, 0, 4))
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(8f);
            CursorVisible = false;

            chunkBuffer = new ChunkRenderBuffer();
            GameManager.Start(this);
        }

        //Update physics
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GameManager.Time += (float)e.Time;
            GameManager.InputManager.ProcessInput(Focused);
            Title = RenderFrequency + "";

            Debug.CSV.Add("fps", new string[] { GameManager.Time.ToString(), RenderFrequency.ToString() });

            //Update all GameObjects
            foreach (GameObject g in chunkBuffer.GetGameObjects())
                g.OnUpdate(GameManager.Time - (float)e.Time);

            foreach (GameObject g in gameObjects)
                g.OnUpdate(GameManager.Time - (float)e.Time);
        }

        //Update Rendering
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

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
            GameManager.InputManager.lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        #region GameObject management
        public void AddGameObject(GameObject g)
        {
            switch (g.Type)
            {
                case GameObjectType.ENVIROMENT:
                case GameObjectType.ENTITY:
                    chunkBuffer.AddGameObject(new GameObject[] { g });
                    break;
                case GameObjectType.EMPTY:
                    gameObjects.Add(g);
                    break;
                default:
                    Debug.LogError("Object type not known");
                    break;
            }
        }

        public void RemoveGameObject(GameObject g)
        {
            switch (g.Type)
            {
                case GameObjectType.ENVIROMENT:
                    chunkBuffer.RemoveGameObject(new GameObject[] { g });
                    break;
                case GameObjectType.EMPTY:
                    gameObjects.Remove(g);
                    break;
                default:
                    Debug.LogError("Object type not known");
                    break;
            }
        }
        #endregion
    }
}