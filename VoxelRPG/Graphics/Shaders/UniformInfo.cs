using OpenTK.Graphics.OpenGL;

namespace VoxelRPG.Graphics.Shaders
{
    public class UniformInfo
    {
        public string name = "";
        public int address = -1;
        public int size = 0;
        public ActiveUniformType type;
    }
}
