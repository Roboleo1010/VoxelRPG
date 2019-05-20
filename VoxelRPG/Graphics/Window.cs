using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using VoxelRPG.Game;
using VoxelRPG.Graphics.Shaders;
using VoxelRPG.Graphics.Meshes;
using VoxelRPG.Input;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Graphics
{
    public class Window : GameWindow
    {
        //References
        InputManager inputManager;
        Camera camera = new Camera();
        GameManager gameManager;

        //Shader
        Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        readonly string activeShader = "default";

        //Buffers
        int ibo_elements;

        //Data
        Vector3[] vertexData;
        Vector3[] colorData;
        int[] indiceData;

        public List<Mesh> meshes = new List<Mesh>();

        public Window() : base(1024, 724, new GraphicsMode(32, 24, 0, 4))
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitGame();
            InitGraphics();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            gameManager.Time += (float)e.Time;
            inputManager.ProcessInput(Focused);

            //Get Mesh data
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            int vertcount = 0;

            foreach (Mesh m in meshes)
            {
                vertices.AddRange(m.GetVertices().ToList());
                indices.AddRange(m.GetIndices(vertcount).ToList());
                colors.AddRange(m.GetColors().ToList());
                vertcount += m.VertexCount;
            }

            vertexData = vertices.ToArray();
            indiceData = indices.ToArray();
            colorData = colors.ToArray();

            Title = string.Format("Vertex count: {0}  -  Trinagle count: {1}  -  FPS: {2}FPS", vertices.Count, indices.Count / 3, RenderFrequency);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indiceData.Length * sizeof(int)), indiceData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * Vector3.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            foreach (Mesh m in meshes) //100ms
            {
                m.CalculateModelMatrix();
                m.ViewProjectionMatrix = camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height,
                                         Constants.Camera.NearClippingPane, Constants.Camera.FarClippingPane);
                m.ModelViewProjectionMatrix = m.ModelMatrix * m.ViewProjectionMatrix;
            }

            GL.UseProgram(shaders[activeShader].ProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            shaders[activeShader].EnableVertexAttribArrays();

            //Renders Meshes individually
            int indiceAt = 0;

            foreach (Mesh m in meshes)
            {
                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref m.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, m.IndiceCount, DrawElementsType.UnsignedInt, indiceAt * sizeof(uint));

                indiceAt += m.IndiceCount;
            }

            shaders[activeShader].DisableVertexAttribArrays();

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
            inputManager.lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        void InitGame()
        {
            gameManager = new GameManager(this);
            inputManager = new InputManager(this, camera);
            CursorVisible = false;

            gameManager.world.GenerateChunkAt(new Vector2Int(0, 0));
            gameManager.world.GenerateChunkAt(new Vector2Int(1, 0));
        }

        void InitGraphics()
        {
            GL.ClearColor(Color.CornflowerBlue); //BG Color
            GL.PointSize(5);

            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new ShaderProgram("Graphics/Shaders/vertex.glsl", "Graphics/Shaders/fragment.glsl", true));
        }
    }
}
