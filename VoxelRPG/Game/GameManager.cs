using VoxelRPG.Game.GameWorld;

namespace VoxelRPG.Game
{
    public class GameManager
    {
        public World world;

        public double Time = 0;


        public GameManager()
        {
            world = new World();
        }
    }
}
