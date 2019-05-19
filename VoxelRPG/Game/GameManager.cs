using VoxelRPG.Game.GameWorld;
using VoxelRPG.Graphics;

namespace VoxelRPG.Game
{
    public class GameManager
    {
        public World world;
        public Window window;

        public double Time = 0;

        public GameManager(Window window)
        {
            this.window = window;
            Constants.gameManager = this;

            world = new World();            
        }
    }
}
