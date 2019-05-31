using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using VoxelRPG.Engine.Diagnosatics;
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
        int vbo_position;
        int vbo_color;
        int vbo_mview;
        int ibo_elements;

        Vector3[] vertexData;
        Vector3[] colorData;
        int[] indiceData;

        ShaderInfo shaderInfo;

        public ChunkRenderBuffer()
        {
            shaderInfo = GenerateShader();

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
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
            shaderInfo.Uniform_modelview = GL.GetUniformLocation(shaderInfo.ShaderProgramID, "modelview");

            return shaderInfo;
        }

        public override void GatherData()
        {
            if (!IsChanged)
                return;

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;

            foreach (GameObject o in GetGameObjects())
            {
                Renderer r = (Renderer)o.GetComponent(ComponentType.Renderer);
                if (r == null)
                    continue;

                Mesh m = r.mesh;
                if (m == null)
                    continue;

                Debug.LogInfo(r.mesh.Transform.Position);

                vertices.AddRange(m.GetVertices());
                indices.AddRange(m.GetIndices(vertcount));
                colors.AddRange(m.GetColors());
                vertcount += m.VertexCount;
            }

            vertexData = vertices.ToArray();
            indiceData = indices.ToArray();
            colorData = colors.ToArray();
            IsChanged = false;

            Debug.LogInfo(string.Format("Recalculated Chunks. {0} vertices", vertices.Count.ToString()));
        }

        public override void BindBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * Vector3.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInfo.Attribute_vertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInfo.Attribute_vertexColor, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indiceData.Length * sizeof(int)), indiceData, BufferUsageHint.StaticDraw);
        }

        public override void Render()
        {
            GL.UseProgram(shaderInfo.ShaderProgramID);

            GL.EnableVertexAttribArray(shaderInfo.Attribute_vertexPosition);
            GL.EnableVertexAttribArray(shaderInfo.Attribute_vertexColor);

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
        }
    }
}
