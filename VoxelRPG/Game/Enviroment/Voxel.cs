using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums.Chunk;

namespace VoxelRPG.Game.Enviroment
{
    public class Voxel
    {
        public Vector3Int Position { get; set; }
        public BlockType Type { get; set; }
        public bool IsTransparent { get; set; } //TODO: Auslagern wg. Speicherplatz
        public bool HasCollider { get; set; } //TODO: Auslagern wg. Speicherplatz

        Chunk parentChunk;

        public Voxel(Vector3Int voxelWorldPos, BlockType blockType, Chunk parent)
        {
            Position = voxelWorldPos;
            Type = blockType;

            parentChunk = parent;

            if (blockType == BlockType.AIR)
                IsTransparent = true;
            else
            {
                parentChunk.isEmpty = false;
                IsTransparent = false;
                HasCollider = true;
            }
        }
    }
}