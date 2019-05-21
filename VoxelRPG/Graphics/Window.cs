using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using VoxelRPG.Game;
using VoxelRPG.Graphics.Meshes;
using VoxelRPG.Graphics.Shaders;
using VoxelRPG.Input;

namespace VoxelRPG.Graphics
{
    public class Window : GameWindow
    {
        //References
        InputManager inputManager;
        public Camera camera = new Camera();
        GameManager gameManager;

        public List<Mesh> meshes = new List<Mesh>();

        public Window() : base(1024, 724, new GraphicsMode(32, 24, 0, 4))
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitGame();
            InitGraphics();

            meshes.Add(new Cube());
            meshes.Add(new Cube()
            {
                Position = new Vector3(5, 2, 1)
            });
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            gameManager.Time += (float)e.Time;
            inputManager.ProcessInput(Focused);

            foreach (Mesh m in meshes)
                m.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Empty Buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Mesh m in meshes)
                m.OnRenderFrame(e);

            GL.Flush(); //Forces asap executionof all pending functions            
            SwapBuffers(); //Sets prepared buffer to be the active buffer
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
            inputManager.lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        void InitGame()
        {
            gameManager = new GameManager(this);
            inputManager = new InputManager(this, camera);

            CursorVisible = false;
        }

        void InitGraphics()
        {
            //Sets background color
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5);
            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, Width, Height);

            ShaderInfo.ShaderProgramID = GL.CreateProgram();

            //Loads and compiles shaders. Attaches them to the given Program
            ShaderHelper.LoadShader("Graphics/Shaders/vertex.glsl", ShaderType.VertexShader, ShaderInfo.ShaderProgramID, out ShaderInfo.VertexShaderID);
            ShaderHelper.LoadShader("Graphics/Shaders/fragment.glsl", ShaderType.FragmentShader, ShaderInfo.ShaderProgramID, out ShaderInfo.FragmentShaderID);

            GL.LinkProgram(ShaderInfo.ShaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(ShaderInfo.ShaderProgramID));

            //Binds shader attributes to application code via the returned indices
            ShaderInfo.Attribute_vertexPosition = GL.GetAttribLocation(ShaderInfo.ShaderProgramID, "vPosition");
            ShaderInfo.Attribute_vertexColor = GL.GetAttribLocation(ShaderInfo.ShaderProgramID, "vColor");
            ShaderInfo.Uniform_modelview = GL.GetUniformLocation(ShaderInfo.ShaderProgramID, "modelview");
        }
    }
}
