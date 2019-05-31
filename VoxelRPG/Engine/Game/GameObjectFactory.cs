using OpenTK;
using System.Linq;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Engine.Manager.Models;
using VoxelRPG.Game.Graphics.Meshes;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Game
{
    public static class GameObjectFactory
    {
        public static GameObject Cube(Vector3 pos, Vector3 rot, Vector3 scale)
        {
            GameObject gameObject = new GameObject();
            gameObject.Type = GameObjectType.ENVIROMENT;
            gameObject.Transform.Position = pos;
            gameObject.Transform.Rotation = rot;
            gameObject.Transform.Scale = scale;
            Renderer r = (Renderer)gameObject.AddComponent<Renderer>(ComponentType.Renderer);
            r.mesh = new CubeMesh();
            r.mesh.Transform = gameObject.Transform;

            return gameObject;
        }

        public static GameObject Model(Vector3 pos, Vector3 rot, Vector3 scale, string name)
        {
            Model model = ModelManager.models[name];

            GameObject gameObject = new GameObject();
            gameObject.Type = GameObjectType.ENVIROMENT;
            gameObject.Transform.Position = pos;
            gameObject.Transform.Rotation = rot;
            gameObject.Transform.Scale = scale;
            Renderer r = (Renderer)gameObject.AddComponent<Renderer>(ComponentType.Renderer);

            Mesh mesh = new VoxelMesh();
            mesh.Copy(model.mesh);
            mesh.Transform = gameObject.Transform;
            r.mesh = mesh;

            return gameObject;
        }
    }
}
