using OpenTK;
using System;
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
            GenerateChunkAt(WorldHelper.ConvertFromWorldSpaceToChunkSpace(GameManager.player.Transform.Position));
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

        public void GenerateAround(Vector3 pos)
        {
            Vector3Int chunkPos = WorldHelper.ConvertFromWorldSpaceToChunkSpace(pos);
 

            int radius = 4;
            for (int x = -radius; x < radius; x++)
                for (int z = -radius; z < radius; z++)
                    // if ((Math.Pow(x - chunkPos.X, 2) + Math.Pow(z - chunkPos.Z, 2)) < Math.Pow(radius, 2))  //Check if point is in circle: (x - center_x)^2 + (y - center_y)^2 < radius^2
                    GenerateChunkAt(new Vector3Int(chunkPos.X + x, 0, chunkPos.Z + z));
        }

        public Chunk GetChunkFromWorldSpace(Vector3 pos)
        {
            chunks.TryGetValue(WorldHelper.ConvertFromWorldSpaceToChunkSpace(pos), out Chunk c);
            return c;
        }

        public Chunk GetChunkFromChunkSpace(Vector3Int pos)
        {
            chunks.TryGetValue(pos, out Chunk c);
            return c;
        }
    }
}
