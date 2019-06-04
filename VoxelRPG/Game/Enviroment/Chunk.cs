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
        Voxel[,,] voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Height, Constants.World.Chunk.Size];
        public bool IsEmpty = false; //TODO
        public VoxelMesh mesh;

        public Chunk(Vector3Int pos)
        {
            Vector3Int chunkWorldPos = WorldHelper.ConvertFromChunkSpaceToWorldSpace(pos);
            Transform.Position = new Vector3(chunkWorldPos.X, chunkWorldPos.Y, chunkWorldPos.Z);
        }

        public void Generate()
        {
            WorldGenerator g = new WorldGenerator(1);

            voxels = g.Generate(Transform.RoundedPosition); //GameManager.generator.
        }

        public void Build()
        {
            mesh = new VoxelMesh(voxels);
            mesh.Transform = this.Transform;
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
            if (pos.X < 0 || pos.Y < 0 || pos.Z < 0 ||
                pos.X > Constants.World.Chunk.Size ||
                pos.Y > Constants.World.Chunk.Height ||
                pos.Z > Constants.World.Chunk.Size)
                return null;
            return voxels[pos.X, pos.Y, pos.Z];
        }
    }
}