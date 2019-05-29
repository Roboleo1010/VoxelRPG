using System;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Engine.Game;
using VoxelRPG.Game.Generation;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Game.Enviroment
{
    public class Chunk : GameObject
    {
        public Vector3Int chunkPos;
        public Vector3Int chunkWorldPos;

        public Voxel[,,] voxels = new Voxel[Constants.World.Chunk.Size, Constants.World.Chunk.Size, Constants.World.Chunk.Size];
        int[,] heightData = new int[Constants.World.Chunk.Size, Constants.World.Chunk.Size];

        public bool isEmpty = true;

        public ChunkMesh mesh;
        WorldGenerator generator = new WorldGenerator();

        public Chunk(Vector3Int pos)
        {
            chunkPos = pos;
            chunkWorldPos = new Vector3Int(pos.X * Constants.World.Chunk.Size,
                                                  pos.Y * Constants.World.Chunk.Size,
                                                  pos.Z * Constants.World.Chunk.Size);
        }

        public void Generate()
        {
            Vector3Int voxelWorldPos;

            for (int x = 0; x < Constants.World.Chunk.Size; x++)
                for (int z = 0; z < Constants.World.Chunk.Size; z++)
                {
                    heightData[x, z] = generator.GetHeight(x + chunkWorldPos.X, z + chunkWorldPos.Z);
                    for (int y = 0; y < Constants.World.Chunk.Size; y++)
                    {
                        voxelWorldPos = new Vector3Int(chunkWorldPos.X + x, chunkWorldPos.Y + y, chunkWorldPos.Z + z);
                        voxels[x, y, z] = new Voxel(voxelWorldPos, generator.GetBlockType(voxelWorldPos, heightData[x, z]), this);
                    }
                }
        }

        public void Build()
        {
            mesh = new ChunkMesh(this);
            mesh.Build();
        }

        public void Queue()
        {
            GameManager.window.AddGameObject(this);
        }

        protected override Mesh GetMeshVirtual()
        {
            if (isEmpty)
                return null;
            else
                return mesh;
        }
    }
}