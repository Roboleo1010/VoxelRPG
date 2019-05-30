using OpenTK;
using System;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Enviroment
{
    public class Voxel
    {
        public Vector3Int Position { get; set; }
        public BlockType Type { get; set; }
        public Vector3 Color { get; set; }
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
                parentChunk.IsEmpty = false;
                IsTransparent = false;
                HasCollider = true;
            }

            if (Type == BlockType.GRASS)
                Color = Constants.World.Chunk.Colors.Grass;
            else if (Type == BlockType.STONE)
                Color = Constants.World.Chunk.Colors.Stone;
            else
                Color = Constants.World.Chunk.Colors.Snow;

            Color = new Vector3((float)(Color.X + GameManager.Random.NextDouble() * 0.08f), (float)(Color.Y + GameManager.Random.NextDouble() * 0.08f), (float)(Color.Z + GameManager.Random.NextDouble() * 0.08f));
        }
    }
}