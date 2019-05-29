using System.Collections.Concurrent;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Enviroment
{
    public class World
    {
        ConcurrentDictionary<string, Chunk> chunks = new ConcurrentDictionary<string, Chunk>();

        public World()
        {

        }

        public Chunk GenerateChunkAt(Vector3Int position)
        {
            Chunk chunk;
            string chunkName = position.ToString();

            if (!chunks.TryGetValue(chunkName, out chunk))
            {
                chunk = new Chunk(position);
                chunk.Generate();
                chunk.Build();
                chunks.TryAdd(chunkName, chunk);
            }

            return chunk;
        }
    }
}
