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

        //public Chunk GetCurrentChunk()
        //{
        //    return GameManager.World.GetChunkFromChunkSpace(new Vector3Int(
        //        Transform.RoundedPosition.X / Constants.World.Chunk.Size,
        //        Transform.RoundedPosition.Y / Constants.World.Chunk.Height,
        //        Transform.RoundedPosition.Z / Constants.World.Chunk.Size));
        //}

        //public Voxel GetCurrentVoxel()
        //{
        //    //player Pos
        //    int pX = Transform.RoundedPosition.X;
        //    int pY = Transform.RoundedPosition.Y;
        //    int pZ = Transform.RoundedPosition.Z;

        //    //Chunk pos in chunk Space
        //    int cX = pX / Constants.World.Chunk.Size;
        //    int cY = pY / Constants.World.Chunk.Height;
        //    int cZ = pZ / Constants.World.Chunk.Size;
            

        //    return null;
        //}
    }
}