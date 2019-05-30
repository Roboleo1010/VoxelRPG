using OpenTK;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Game
{
    public static class GameObjectFactory
    {
        public static GameObject Cube(Vector3 pos, Vector3 rot, Vector3 scale)
        {
            GameObject cube = new GameObject();
            cube.Transform.Position = pos;
            cube.Transform.Rotation = rot;
            cube.Transform.Scale = scale;
            Renderer r = (Renderer)cube.AddComponent<Renderer>(ComponentType.Renderer);
            r.mesh = new Cube()
            {
                Transform = cube.Transform
            };

            return cube;
        }
    }
}
