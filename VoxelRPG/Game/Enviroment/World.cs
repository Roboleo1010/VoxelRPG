using System.Collections.Concurrent;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Enviroment
{
    public class World
    {
        ConcurrentDictionary<Vector3Int, Chunk> chunks = new ConcurrentDictionary<Vector3Int, Chunk>();

        public World() { }

        public void Init()
        {
            int size = 5;
            for (int x = -size; x < size; x++)
                for (int z = -size; z < size; z++)
                    GenerateChunkAt(new Vector3Int(x, 0, z));
        }

        public Chunk GenerateChunkAt(Vector3Int position)
        {
            Chunk chunk;

            if (!chunks.TryGetValue(position, out chunk))
            {
                chunk = new Chunk(position);
                chunk.Type = GameObjectType.ENVIROMENT;
                chunk.Name = "Chunk: " + position.ToString();
                chunk.Generate();
                chunk.Build();
                chunk.Queue();
                chunks.TryAdd(position, chunk);
            }

            return chunk;
        }

        public Chunk GetChunk(Vector3Int pos)
        {
            chunks.TryGetValue(pos, out Chunk c);
            return c;
        }
    }
}
