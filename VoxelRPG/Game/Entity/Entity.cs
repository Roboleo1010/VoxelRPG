using System;
using VoxelRPG.Engine.Game;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Utilitys;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Game.Entity
{
    public class Entity : GameObject
    {
        #region Component Propertys
        public Rigidbody Rigidbody;
        #endregion

        public Entity()
        {
            Rigidbody = (Rigidbody)AddComponent<Rigidbody>(ComponentType.Rigidbody);
        }

        public Chunk GetCurrentChunk()
        {
            return GameManager.world.GetChunk(new Vector3Int((int)(Transform.Position.X / Constants.World.Chunk.Size),
                                                             (int)(Transform.Position.Y / Constants.World.Chunk.Size),
                                                             (int)(Transform.Position.Z / Constants.World.Chunk.Size)));
        }

        public Voxel GetCurrentVoxel()
        {
            throw new NotImplementedException();
        }
    }
}