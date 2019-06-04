using OpenTK;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Enviroment
{
    public class Voxel
    {
        public Vector3Int Position { get; set; }
        public Vector3 Color { get; set; }

        public Voxel(Vector3Int voxelWorldPos, Vector3 color)
        {
            Position = voxelWorldPos;
            Color = color; 
        }
    }
}