using Newtonsoft.Json;
using OpenTK;

namespace VoxelRPG.Engine.Manager.Models
{
    public class ModelColor
    {
        public string name;
        public Vector3 color;

        [JsonConstructor]
        public ModelColor(string name, float r, float g, float b)
        {
            this.name = name;
            color = new Vector3(r, g, b);
        }
    }
}
