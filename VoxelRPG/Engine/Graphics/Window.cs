using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Engine.Shaders;
using VoxelRPG.Game;
using VoxelRPG.Game.Entity;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Input;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Graphics
{
    public class Window : GameWindow
    {
        #region Rendering
        int vbo_position;
        int vbo_color;
        int vbo_mview;
        int ibo_elements;
        Vector3[] vertdata;
        Vector3[] coldata;
        int[] indicedata;
        #endregion

        #region Game References
        Player player;
        List<GameObject> gameObjects = new List<GameObject>();
        #endregion

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
            GameManager.inputManager.ProcessInput(Focused);

            Title = RenderFrequency + "";

            float deltaTime = GameManager.Time - (float)e.Time;
            foreach (GameObject g in gameObjects)
                g.OnUpdate(deltaTime);

            //TODO CACHE!
            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;

            foreach (GameObject o in gameObjects)
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                verts.AddRange(m.GetVertices());
                inds.AddRange(m.GetIndices(vertcount));
                colors.AddRange(m.GetColors());
                vertcount += m.VertexCount;
            }

            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            //end todo

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(ShaderInfo.Attribute_vertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(ShaderInfo.Attribute_vertexColor, 3, VertexAttribPointerType.Float, true, 0, 0);

            foreach (GameObject o in gameObjects)
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                m.CalculateModelMatrix();
                m.ViewProjectionMatrix = player.camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f,
                                         GameManager.window.ClientSize.Width / (float)GameManager.window.ClientSize.Height,
                                         Constants.Camera.NearClippingPane, Constants.Camera.FarClippingPane);

                m.ModelViewProjectionMatrix = m.ModelMatrix * m.ViewProjectionMatrix;
            }

            GL.UseProgram(ShaderInfo.ShaderProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(ShaderInfo.Attribute_vertexPosition);
            GL.EnableVertexAttribArray(ShaderInfo.Attribute_vertexColor);

            int indiceAt = 0;

            foreach (GameObject o in gameObjects)
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                GL.UniformMatrix4(ShaderInfo.Uniform_modelview, false, ref m.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, m.IndiceCount, DrawElementsType.UnsignedInt, indiceAt * sizeof(uint));
                indiceAt += m.IndiceCount;
            }

            GL.DisableVertexAttribArray(ShaderInfo.Attribute_vertexPosition);
            GL.DisableVertexAttribArray(ShaderInfo.Attribute_vertexColor);

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

            gameObjects.Add(player);
            GameManager.inputManager = new InputManager(this, player);
            CursorVisible = false;

            GameObject cube = new GameObject();
            Renderer r = (Renderer)cube.AddComponent<Renderer>(ComponentType.Renderer);
            r.mesh = new Cube();
            gameObjects.Add(cube);

            //Generate world
            int size = 2;
            for (int x = -size; x < size; x++)
                for (int z = -size; z < size; z++)
                    GameManager.world.GenerateChunkAt(new Vector3Int(x, 0, z));
        }

        void InitGraphics()
        {
            ShaderInfo.ShaderProgramID = GL.CreateProgram();

            ShaderHelper.LoadShader("Shaders/vertex.glsl", ShaderType.VertexShader, ShaderInfo.ShaderProgramID, out ShaderInfo.VertexShaderID);
            ShaderHelper.LoadShader("Shaders/fragment.glsl", ShaderType.FragmentShader, ShaderInfo.ShaderProgramID, out ShaderInfo.FragmentShaderID);

            GL.LinkProgram(ShaderInfo.ShaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(ShaderInfo.ShaderProgramID));

            ShaderInfo.Attribute_vertexPosition = GL.GetAttribLocation(ShaderInfo.ShaderProgramID, "vPosition");
            ShaderInfo.Attribute_vertexColor = GL.GetAttribLocation(ShaderInfo.ShaderProgramID, "vColor");
            ShaderInfo.Uniform_modelview = GL.GetUniformLocation(ShaderInfo.ShaderProgramID, "modelview");

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
            GL.GenBuffers(1, out ibo_elements);

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(8f);
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
