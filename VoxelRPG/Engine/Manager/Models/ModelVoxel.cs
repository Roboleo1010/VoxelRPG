using Newtonsoft.Json;
using OpenTK;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Manager.Models
{
    public class ModelVoxel
    {
        public string colorName;
        public Vector3Int position;
        public Vector3 color;

        [JsonConstructor]
        public ModelVoxel(string color, int x, int y, int z)
        {
            colorName = color;
            position = new Vector3Int(x, y, z);
            this.color = Vector3.Zero;
        }
    }
}
