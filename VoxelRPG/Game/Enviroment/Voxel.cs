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
            Color = new Vector3((float)(color.X + GameManager.Random.NextDouble() * 0.08f), (float)(color.Y + GameManager.Random.NextDouble() * 0.08f), (float)(color.Z + GameManager.Random.NextDouble() * 0.08f));
        }
    }
}