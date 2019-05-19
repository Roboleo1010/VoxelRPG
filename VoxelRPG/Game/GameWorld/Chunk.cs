using VoxelRPG.Game.Generation;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.GameWorld
{
    public class Chunk
    {
        public Vector2Int Position = new Vector2Int(); // in world

        bool[,,] isSolid = new bool[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Height];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        public Chunk(Vector2Int pos)
        {
            Position = pos;
        }

        public void GatherData()
        {
            WorldGeneration generator = new WorldGeneration();

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    heightData[x, z] = generator.GetHeight(x + Position.X, z + Position.Z);
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        if (y > heightData[x, z])
                            isSolid[x, z, y] = false;
                        else
                            isSolid[x, z, y] = true;
                    }
                }
        }

        public void Build()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                    for (int y = 0; y < Constants.World.Chunk.Height; y++)
                    {
                        GetMeshData(x, z, y);
                    }
        }

        public void Draw()
        {

        }

        bool IsSolid(int x, int z, int y)
        {
            return isSolid[x , z, y];
        }

        private void GetMeshData(int x, int z, int y)
        {

        }
    }
}
