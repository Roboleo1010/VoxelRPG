using System.Collections.Concurrent;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.GameWorld
{
    public class World
    {
        ConcurrentDictionary<string, Chunk> chunks = new ConcurrentDictionary<string, Chunk>();

        public World()
        {

        }

        public Chunk GenerateChunkAt(Vector2Int position) //Position of chunk, not in world
        {
            Vector2Int chunkPosition = new Vector2Int(position.X * Constants.World.Chunk.Size,
                                                      position.Z * Constants.World.Chunk.Size);

            Chunk chunk;
            string chunkName = position.ToString();

            if (!chunks.TryGetValue(chunkName, out chunk))
            {
                chunk = new Chunk(chunkPosition);
                chunk.GatherData();
                chunk.Build();
                chunk.Draw();
                chunks.TryAdd(chunkName, chunk);
            }

            return chunk;
        }
    }
}
