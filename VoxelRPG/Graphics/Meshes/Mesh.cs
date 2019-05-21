using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using VoxelRPG.Game;
using VoxelRPG.Graphics.Shaders;

namespace VoxelRPG.Graphics.Meshes
{
    public abstract class Mesh
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public int VertexCount;
        public int IndiceCount;
        public int ColorCount;
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        //Vertex Buffer Objects
        int vbo_position;
        int vbo_color;
        int ibo_elements;

        public Mesh()
        {
            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out ibo_elements);
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            //Bind POSITION Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);  //Preape buffer for writing
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(GetVertices().Length * Vector3.SizeInBytes), GetVertices(), BufferUsageHint.StaticDraw); //Write into buffer
            GL.VertexAttribPointer(ShaderInfo.Attribute_vertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0); //For which shader attribute

            //Bind COLOR Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(GetColors().Length * Vector3.SizeInBytes), GetColors(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(ShaderInfo.Attribute_vertexColor, 3, VertexAttribPointerType.Float, true, 0, 0);

            //Bind INDEX Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(GetIndices().Length * sizeof(int)), GetIndices(), BufferUsageHint.StaticDraw);

            CalculateModelMatrix();
            ViewProjectionMatrix = GameManager.window.camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, GameManager.window.ClientSize.Width / (float)GameManager.window.ClientSize.Height, 1.0f, 40.0f);
            ModelViewProjectionMatrix = ModelMatrix * ViewProjectionMatrix;

            //Apply Shaders
            GL.UseProgram(ShaderInfo.ShaderProgramID);
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            //TODO ??
            GL.EnableVertexAttribArray(ShaderInfo.Attribute_vertexPosition);
            GL.EnableVertexAttribArray(ShaderInfo.Attribute_vertexColor);


            //Draw all Meshes individually
            GL.UniformMatrix4(ShaderInfo.Uniform_modelview, false, ref ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, IndiceCount, DrawElementsType.UnsignedInt, 0);

            //TODO ??
            GL.DisableVertexAttribArray(ShaderInfo.Attribute_vertexPosition);
            GL.DisableVertexAttribArray(ShaderInfo.Attribute_vertexColor);
        }

        public abstract Vector3[] GetVertices();
        public abstract int[] GetIndices();
        public abstract Vector3[] GetColors();
        public abstract void CalculateModelMatrix();
    }
}
