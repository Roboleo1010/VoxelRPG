using OpenTK;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Enviroment
{
    public static class WorldHelper
    {
        public static Vector3Int ConvertFromWorldSpaceToChunkSpace(Vector3 pos)
        {
            int cX, cY, cZ;

            cX = (int)pos.X / Constants.World.Chunk.Size;
            if (pos.X < 0)
                cX--;

            cY = (int)pos.Y / Constants.World.Chunk.Height;
            if (pos.Y < 0)
                cY--;

            cZ = (int)pos.Z / Constants.World.Chunk.Size;
            if (pos.Z < 0)
                cZ--;

            return new Vector3Int(cX, cY, cZ);
        }

        public static Vector3Int ConvertFromChunkSpaceToWorldSpace(Vector3Int pos)
        {
            return new Vector3Int(pos.X * Constants.World.Chunk.Size,
                                  pos.Y * Constants.World.Chunk.Height,
                                  pos.Z * Constants.World.Chunk.Size);
        }
    }
}
