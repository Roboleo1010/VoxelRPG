using OpenTK;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;
using Debug = VoxelRPG.Engine.Diagnosatics.Debug;

namespace VoxelRPG.Game.Enviroment
{
    public class World
    {
        public ConcurrentDictionary<Vector3Int, Chunk> chunks = new ConcurrentDictionary<Vector3Int, Chunk>();
        List<Vector3Int> currentlyGenerating = new List<Vector3Int>();
        List<Vector3Int> generatedButNotQueued = new List<Vector3Int>();

        public World() { }

        public void Init()
        {
            GenerateChunkAt(WorldHelper.ConvertFromWorldSpaceToChunkSpace(GameManager.player.Transform.Position));
        }

        public void GenerateChunkAt(Vector3Int position)
        {
            if (!currentlyGenerating.Contains(position) && !chunks.ContainsKey(position))
            {
                currentlyGenerating.Add(position);

                ThreadStart starter = new ChunkGenerationThread(position).DoWork;
                starter += () =>
                {
                    currentlyGenerating.Remove(position);
                    generatedButNotQueued.Add(position);
                };

                Thread thread = new Thread(starter)
                {
                    Name = "Generate Chunk: " + position.ToString(),
                    IsBackground = true
                };

                thread.Start();
            }
        }

        public void QueueGeneratedChunks()
        {
            Chunk chunk;

            if (generatedButNotQueued.Count > 0)
            {
                Vector3Int pos = generatedButNotQueued[0];

                if (chunks.TryGetValue(pos, out chunk))
                    chunk.Queue();
                else
                    Debug.LogError("Cound not generate Chunk " + pos.ToString());

                generatedButNotQueued.Remove(pos);
            }
        }

        public void GenerateAround(Vector3 pos)
        {
            Vector3Int chunkPos = WorldHelper.ConvertFromWorldSpaceToChunkSpace(pos);
            //
            int radius = 2;
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

    public class ChunkGenerationThread
    {
        Vector3Int position;

        public ChunkGenerationThread(Vector3Int pos)
        {
            position = pos;
        }

        public void DoWork()
        {
            Chunk chunk = new Chunk(position)
            {
                Type = GameObjectType.ENVIROMENT,
                Name = "Chunk: " + position.ToString()
            };

            chunk.Generate();
            chunk.Build();

            GameManager.world.chunks.TryAdd(position, chunk);
        }
    }
}
