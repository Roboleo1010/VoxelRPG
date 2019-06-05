using VoxelRPG.Game.Enviroment;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Generation
{
    public class ChunkGenerationThread
    {
        Vector3Int position;

        public ChunkGenerationThread(Vector3Int pos)
        {
            position = pos;
        }

        public void DoWork()
        {
            Chunk chunk = new Chunk(position);
            chunk.Generate();
            chunk.Build();

            GameManager.World.chunks.TryAdd(position, chunk);
        }
    }
}
