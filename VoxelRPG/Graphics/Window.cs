using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using VoxelRPG.Game;
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

        //Shader IDs
        int shaderProgramID;
        int vertexShaderID;
        int fragmentShaderID;

        //Shader Attributes
        int attribute_vertexColor;
        int attribute_vertexPosition;
        int uniform_modelview;

        //Vertex Buffer Objects
        int vbo_position;
        int vbo_color;
        int ibo_elements;

        //Data
        Vector3[] vertexData;
        Vector3[] colorData;
        float[] timeData;
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

            Title = gameManager.Time + "";

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            int vertcount = 0;

            //alle Meshdaten sammeln und vereinigen
            foreach (Mesh m in meshes)
            {
                vertices.AddRange(m.GetVertices().ToList());
                indices.AddRange(m.GetIndices(vertcount).ToList());
                colors.AddRange(m.GetColors().ToList());
                vertcount += m.VertexCount;
            }

            //Convert Lists to Arrays
            vertexData = vertices.ToArray();
            indiceData = indices.ToArray();
            colorData = colors.ToArray();

            //Bind POSITION Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);  //Preape buffer for writing
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * Vector3.SizeInBytes), vertexData, BufferUsageHint.StaticDraw); //Write into buffer
            GL.VertexAttribPointer(attribute_vertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0); //For which shader attribute

            //Bind COLOR Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vertexColor, 3, VertexAttribPointerType.Float, true, 0, 0);

            //Bind INDEX Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indiceData.Length * sizeof(int)), indiceData, BufferUsageHint.StaticDraw);

            //Calculate MODEL MATRIX for each Mesh
            foreach (Mesh m in meshes)
            {
                m.CalculateModelMatrix();
                m.ViewProjectionMatrix = camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
                m.ModelViewProjectionMatrix = m.ModelMatrix * m.ViewProjectionMatrix;
            }

            //Apply Shaders
            GL.UseProgram(shaderProgramID);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Empty Buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //TODO ??
            GL.EnableVertexAttribArray(attribute_vertexPosition);
            GL.EnableVertexAttribArray(attribute_vertexColor);

            int indiceAt = 0;

            //Draw all Meshes individually
            foreach (Mesh m in meshes)
            {
                GL.UniformMatrix4(uniform_modelview, false, ref m.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, m.IndiceCount, DrawElementsType.UnsignedInt, indiceAt * sizeof(uint));

                indiceAt += m.IndiceCount;
            }

            //TODO ??
            GL.DisableVertexAttribArray(attribute_vertexPosition);
            GL.DisableVertexAttribArray(attribute_vertexColor);

            //Forces asap executionof all pending functions
            GL.Flush();

            //Sets prepared buffer to be the active buffer
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

            //Constrains cursor to Window
            CursorVisible = false;

            gameManager.world.GenerateChunkAt(new Vector2Int(0, 0));
            gameManager.world.GenerateChunkAt(new Vector2Int(1, 0));
        }

        void InitGraphics()
        {
            //Sets background color
            GL.ClearColor(Color.CornflowerBlue);

            //Sets size for Points. Only useful if Rendermode = Points
            GL.PointSize(5);
            //Enables Depthtesting. Responsible for drawing some verts behind others
            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, Width, Height);

            shaderProgramID = GL.CreateProgram();

            //Loads and compiles shaders. Attaches them to the given Program
            LoadShader("Graphics/Shaders/vertex.glsl", ShaderType.VertexShader, shaderProgramID, out vertexShaderID);
            LoadShader("Graphics/Shaders/fragment.glsl", ShaderType.FragmentShader, shaderProgramID, out fragmentShaderID);

            GL.LinkProgram(shaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgramID));

            //Binds shader attributes to application code via the returned indices
            attribute_vertexPosition = GL.GetAttribLocation(shaderProgramID, "vPosition");
            attribute_vertexColor = GL.GetAttribLocation(shaderProgramID, "vColor");
            uniform_modelview = GL.GetUniformLocation(shaderProgramID, "modelview");

            if (attribute_vertexColor == -1 || attribute_vertexPosition == -1 || uniform_modelview == -1)
                Console.WriteLine("Error binding attributes");

            //Generates buffers
            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out ibo_elements);
        }

        /// <summary>
        /// Attaches the complied shader to the gigven ProgramID
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="programID"></param>
        /// <param name="address"></param>
        void LoadShader(string filename, ShaderType type, int programID, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader reader = new StreamReader(filename))
            {
                GL.ShaderSource(address, reader.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(programID, address);

            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}
