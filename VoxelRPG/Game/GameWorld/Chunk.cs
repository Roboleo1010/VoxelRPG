using VoxelRPG.Game.Generation;
using VoxelRPG.Graphics.Meshes;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums.Chunk;

namespace VoxelRPG.Game.GameWorld
{
    public class Chunk
    {
        public Vector3Int chunkPosition;
        public Vector3Int chunkPositionInWorld;

        public BlockType[,,] blockTypes = new BlockType[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Size];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];
        bool isEmpty = true;

        public ChunkMesh mesh;
        WorldGenerator generator = new WorldGenerator();

        public Chunk(Vector3Int pos)
        {
            chunkPosition = pos;
            chunkPositionInWorld = new Vector3Int(pos.X * Constants.World.Chunk.Size,
                                                  pos.Y * Constants.World.Chunk.Size,
                                                  pos.Z * Constants.World.Chunk.Size);
        }

        public void Generate()
        {
            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    heightData[x, z] = generator.GetHeight(x + chunkPositionInWorld.X, z + chunkPositionInWorld.Z);
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        if (y > heightData[x, z])
                            blockTypes[x, y, z] = BlockType.AIR;
                        else
                        {
                            isEmpty = false;

                            if (y == heightData[x, z])
                                blockTypes[x, y, z] = BlockType.GRASS;
                            else if (y < heightData[x, z])
                                blockTypes[x, y, z] = BlockType.STONE;
                        }
                    }
                }
        }

        public bool Build()
        {
            if (isEmpty)
                return false;

            mesh = new ChunkMesh(this);
            mesh.Build();
            mesh.Render();
            return true;
        }
    }
}