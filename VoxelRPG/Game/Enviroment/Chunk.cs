using OpenTK;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Game.Generation;
using VoxelRPG.Game.Graphics.Meshes;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Enviroment
{
    public class Chunk : GameObject
    {
        public Voxel[,,] Voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Size];
        public bool IsEmpty = true;
        public VoxelMesh mesh;

        public Chunk(Vector3Int pos)
        {
            Vector3Int chunkWorldPos = WorldHelper.ConvertFromChunkSpaceToWorldSpace(pos);
            Transform.Position = new Vector3(chunkWorldPos.X, chunkWorldPos.Y, chunkWorldPos.Z);
        }

        public void Generate()
        {
            Vector3Int voxelWorldPos;
            WorldGenerator generator = new WorldGenerator();

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    int height = generator.GetHeight(x + Transform.RoundedPosition.X, z + Transform.RoundedPosition.Z);
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        voxelWorldPos = new Vector3Int(Transform.RoundedPosition.X + x, Transform.RoundedPosition.Y + y, Transform.RoundedPosition.Z + z);
                        Voxels[x, y, z] = new Voxel(voxelWorldPos, generator.GetBlockType(voxelWorldPos, height), this);
                    }
                }
        }

        public void Build()
        {
            mesh = new VoxelMesh(this, Voxels);
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