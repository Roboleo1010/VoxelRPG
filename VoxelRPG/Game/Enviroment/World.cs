using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VoxelRPG.Engine.Game;
using VoxelRPG.Game.Generation;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;
using Debug = VoxelRPG.Engine.Diagnosatics.Debug;

namespace VoxelRPG.Game.Enviroment
{
    public class World : GameObject
    {
        public ConcurrentDictionary<Vector3Int, Chunk> chunks = new ConcurrentDictionary<Vector3Int, Chunk>();
        List<Vector3Int> currentlyGenerating = new List<Vector3Int>();
        List<Vector3Int> generatedButNotQueued = new List<Vector3Int>();

        public void Start()
        {
            Instantiate("World", GameObjectType.EMPTY);
        }

        protected override void OnUpdateVirtual(float deltaTime)
        {
            GenerateAround(WorldHelper.ConvertFromWorldSpaceToChunkSpace(GameManager.Player.Transform.Position));
            QueueGeneratedChunks();
        }

        public void GenerateChunkAt(Vector3Int posInChunkSpace)
        {
            if (!currentlyGenerating.Contains(posInChunkSpace) && !chunks.ContainsKey(posInChunkSpace))
            {
                currentlyGenerating.Add(posInChunkSpace);

                ThreadStart starter = new ChunkGenerationThread(posInChunkSpace).DoWork;
                starter += () =>
                {
                    currentlyGenerating.Remove(posInChunkSpace);
                    generatedButNotQueued.Add(posInChunkSpace);
                };

                Thread thread = new Thread(starter)
                {
                    Name = "Generate Chunk: " + posInChunkSpace.ToString(),
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
                Vector3Int posInChunkSpace = generatedButNotQueued[0];

                if (chunks.TryGetValue(posInChunkSpace, out chunk))
                    chunk.Render();
                else
                    Debug.LogError("Cound not generate Chunk " + posInChunkSpace.ToString());

                generatedButNotQueued.Remove(posInChunkSpace);
            }
        }

        public void GenerateAround(Vector3Int posInChunkSpace)
        {
            int radius = 2;
            for (int x = -radius; x < radius; x++)
                for (int z = -radius; z < radius; z++)
                    GenerateChunkAt(new Vector3Int(posInChunkSpace.X + x, 0, posInChunkSpace.Z + z));
        }
    }
}
