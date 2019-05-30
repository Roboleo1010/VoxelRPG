using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game.Generation;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Enviroment
{
    public class Chunk : GameObject
    {
        public Vector3Int ChunkPosition;
        public Vector3Int ChunkWorldPosition;
        public Voxel[,,] Voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Size];
        public bool IsEmpty = true;
        public ChunkMesh mesh;

        WorldGenerator generator = new WorldGenerator();

        public Chunk(Vector3Int pos)
        {
            ChunkPosition = pos;
            ChunkWorldPosition = WorldHelper.ConvertFromChunkSpaceToWorldSpace(pos);
        }

        public void Generate()
        {
            Vector3Int voxelWorldPos;

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    int height = generator.GetHeight(x + ChunkWorldPosition.X, z + ChunkWorldPosition.Z);
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        voxelWorldPos = new Vector3Int(ChunkWorldPosition.X + x, ChunkWorldPosition.Y + y, ChunkWorldPosition.Z + z);
                        Voxels[x, y, z] = new Voxel(voxelWorldPos, generator.GetBlockType(voxelWorldPos, height), this);
                    }
                }
        }

        public void Build()
        {
            mesh = new ChunkMesh(this);
            mesh.Build();

            Renderer renderer = (Renderer)AddComponent<Renderer>(ComponentType.Renderer);
            renderer.mesh = mesh;
        }

        public void Queue()
        {
            if (!IsEmpty)
                GameManager.window.AddGameObject(this);
        }

        public Voxel GetVoxel(Vector3Int pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.Z < 0 || pos.X > Constants.World.Chunk.Size |
                pos.Y > Constants.World.Chunk.Size || pos.Z > Constants.World.Chunk.Size)
                return null;
            return Voxels[pos.X, pos.Y, pos.Z];
        }
    }
}