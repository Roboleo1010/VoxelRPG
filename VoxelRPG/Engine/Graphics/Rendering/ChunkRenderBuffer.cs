using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Engine.Shaders;
using VoxelRPG.Game;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Graphics.Rendering
{
    public class ChunkRenderBuffer : RenderBuffer
    {
        List<GameObject> gameObjects = new List<GameObject>();

        int vbo_position;
        int vbo_color;
        int vbo_normal;
        int vbo_mview;
        int ibo_elements;

        Vector3[] vertexData = new Vector3[0];
        Vector3[] colorData = new Vector3[0];
        Vector3[] normalData = new Vector3[0];
        int[] indiceData = new int[0];

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> colors = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        ShaderInfo shaderInfo;

        public ChunkRenderBuffer()
        {
            shaderInfo = GenerateShader();

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_normal);
            GL.GenBuffers(1, out vbo_mview);
            GL.GenBuffers(1, out ibo_elements);
        }

        ShaderInfo GenerateShader()
        {
            shaderInfo = new ShaderInfo();

            shaderInfo.ShaderProgramID = GL.CreateProgram();

            ShaderHelper.LoadShader("Shaders/vertex.glsl", ShaderType.VertexShader, shaderInfo.ShaderProgramID, out shaderInfo.VertexShaderID);
            ShaderHelper.LoadShader("Shaders/fragment.glsl", ShaderType.FragmentShader, shaderInfo.ShaderProgramID, out shaderInfo.FragmentShaderID);

            GL.LinkProgram(shaderInfo.ShaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(shaderInfo.ShaderProgramID));

            shaderInfo.Attribute_vertexPosition = GL.GetAttribLocation(shaderInfo.ShaderProgramID, "vPosition");
            shaderInfo.Attribute_vertexColor = GL.GetAttribLocation(shaderInfo.ShaderProgramID, "vColor");
            shaderInfo.Attribute_vertexNormal = GL.GetAttribLocation(shaderInfo.ShaderProgramID, "vNormal");
            shaderInfo.Uniform_modelview = GL.GetUniformLocation(shaderInfo.ShaderProgramID, "modelview");

            return shaderInfo;
        }

        public override void BindBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * Vector3.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInfo.Attribute_vertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInfo.Attribute_vertexColor, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_normal);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normalData.Length * Vector3.SizeInBytes), normalData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInfo.Attribute_vertexNormal, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indiceData.Length * sizeof(int)), indiceData, BufferUsageHint.StaticDraw);
        }

        public override void Render()
        {
            GL.UseProgram(shaderInfo.ShaderProgramID);

            GL.EnableVertexAttribArray(shaderInfo.Attribute_vertexPosition);
            GL.EnableVertexAttribArray(shaderInfo.Attribute_vertexColor);
            GL.EnableVertexAttribArray(shaderInfo.Attribute_vertexNormal);

            int indiceAt = 0;

            foreach (GameObject o in GetGameObjects())
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                //da sich die Kamera ändern kann, muss die Viewmatrix geupdated werden
                Matrix4 ModelViewProjectionMatrix = m.CalculateModelMatrix() * GameManager.player.camera.GetViewMatrix() *
                                                    Matrix4.CreatePerspectiveFieldOfView(1.3f,
                                                    GameManager.window.ClientSize.Width / (float)GameManager.window.ClientSize.Height,
                                                    Constants.Camera.NearClippingPane, Constants.Camera.FarClippingPane);

                GL.UniformMatrix4(shaderInfo.Uniform_modelview, false, ref ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, m.IndiceCount, DrawElementsType.UnsignedInt, indiceAt * sizeof(uint));
                indiceAt += m.IndiceCount;
            }

            GL.DisableVertexAttribArray(shaderInfo.Attribute_vertexPosition);
            GL.DisableVertexAttribArray(shaderInfo.Attribute_vertexColor);
            GL.DisableVertexAttribArray(shaderInfo.Attribute_vertexNormal);
        }

        public override void AddGameObject(GameObject[] newGameObjects)
        {
            gameObjects.AddRange(newGameObjects);

            int vertcount = vertexData.Length;

            foreach (GameObject o in newGameObjects)
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                vertices.AddRange(m.GetVertices());
                colors.AddRange(m.GetColors());
                normals.AddRange(m.GetNormals());
                indices.AddRange(m.GetIndices(vertcount));
                vertcount += m.VertexCount;
            }

            vertexData = vertices.ToArray();
            colorData = colors.ToArray();
            normalData = normals.ToArray();
            indiceData = indices.ToArray();
        }

        public override void RemoveGameObject(GameObject[] o)
        {
            throw new NotImplementedException();
        }

        public override List<GameObject> GetGameObjects()
        {
            return gameObjects;
        }
    }
}
