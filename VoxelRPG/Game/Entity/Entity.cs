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

        //public Chunk GetCurrentChunk() TODO
        //{
        //    return GameManager.world.GetChunkFromWorldSpace(Transform.Position);
        //}

        //public Voxel GetCurrentVoxel() TODO
        //{
        //    throw new NotImplementedException();
        //}
    }
}