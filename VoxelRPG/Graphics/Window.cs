using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using VoxelRPG.Game;
using VoxelRPG.Graphics.Shaders;
using VoxelRPG.Graphics.Volumes;
using VoxelRPG.Input;

namespace VoxelRPG.Graphics
{
    public class Window : GameWindow
    {
        //References
        InputManager inputManager;
        Camera camera = new Camera();

        //Shader
        Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        readonly string activeShader = "default";

        //Buffers
        int ibo_elements;

        //Data
        Vector3[] vertexData;
        Vector3[] colorData;
        int[] indiceData;
        List<Volume> volumes = new List<Volume>();

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

            GameManager.Time += (float)e.Time;
            inputManager.ProcessInput(Focused);

            Title = RenderFrequency + "fps";

            //Get Volume data
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            int vertcount = 0;

            foreach (Volume v in volumes)
            {
                vertices.AddRange(v.GetVertices().ToList());
                indices.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColors().ToList());
                vertcount += v.VertexCount;
            }

            vertexData = vertices.ToArray();
            indiceData = indices.ToArray();
            colorData = colors.ToArray();

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

            foreach (Volume v in volumes)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height,
                                         Constants.Camera.NearClippingPane, Constants.Camera.FarClippingPane);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
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

            //Renders Volumes individually
            int indiceAt = 0;

            foreach (Volume v in volumes)
            {
                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceAt * sizeof(uint));

                indiceAt += v.IndiceCount;
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
            inputManager = new InputManager(this, camera);
            CursorVisible = false;

            //TODO: Move
            Random rand = new Random();
            for (int i = 0; i < 32; i++)
                for (int j = 0; j < 32; j++)
                {
                    if (i == 0 && j % 2 == 0)
                        continue;

                    Cube c = new Cube(new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()))
                    {
                        Position = new Vector3(i, 0, j)
                    };
                    volumes.Add(c);
                }
        }

        void InitGraphics()
        {
            Title = "Voxel RPG";
            GL.ClearColor(Color.CornflowerBlue); //BG Color
            GL.PointSize(5);

            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new ShaderProgram("Graphics/Shaders/vertex.glsl", "Graphics/Shaders/fragment.glsl", true));
        }
    }
}
