using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace VoxelRPG.Graphics
{
    public static class ShaderHelper
    {
        public static void LoadShader(string filename, ShaderType type, int programID, out int address)
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
