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
        WorldGenerator generator;
        Vector3Int chunkPos;

        public bool IsEmpty = false; //TODO
        public VoxelMesh mesh;

        public Chunk(Vector3Int pos)
        {
            chunkPos = pos;
            Vector3Int chunkWorldPos = WorldHelper.ConvertFromChunkSpaceToWorldSpace(pos);
            Transform.Position = new Vector3(chunkWorldPos.X, chunkWorldPos.Y, chunkWorldPos.Z);
            generator = new WorldGenerator(Transform.RoundedPosition);
        }

        public void Generate()
        {
            generator.Generate();
        }

        public void Build()
        {
            mesh = new VoxelMesh(generator.GetVoxels());
            mesh.Transform = this.Transform;
            mesh.Build();

            Renderer renderer = (Renderer)AddComponent<Renderer>(ComponentType.Renderer);
            renderer.mesh = mesh;
        }

        public void Render()
        {
            if (!IsEmpty)
                Instantiate(chunkPos.ToString(), GameObjectType.ENVIROMENT);
        }

        public Voxel GetVoxel(Vector3Int pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.Z < 0 ||
                pos.X > Constants.World.Chunk.Size ||
                pos.Y > Constants.World.Chunk.Height ||
                pos.Z > Constants.World.Chunk.Size)
                return null;
            return generator.GetVoxel(pos);
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            return generator.GetVoxel(x, y, z);
        }
    }
}