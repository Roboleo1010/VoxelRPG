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
            GameObject g = new GameObject();
            g.Transform.Position = pos;
            g.Transform.Rotation = rot;
            g.Transform.Scale = scale;
            Renderer r = (Renderer)g.AddComponent<Renderer>(ComponentType.Renderer);
            r.mesh = new CubeMesh();
            r.mesh.Transform = g.Transform;

            g.Instantiate("Cube", GameObjectType.ENVIROMENT);

            return g;
        }

        public static GameObject Model(Vector3 pos, Vector3 rot, Vector3 scale, string name)
        {
            Model model = ModelManager.models[name];

            GameObject g = new GameObject();
            g.Transform.Position = pos;
            g.Transform.Rotation = rot;
            g.Transform.Scale = scale;
            Renderer r = (Renderer)g.AddComponent<Renderer>(ComponentType.Renderer);

            Mesh mesh = new VoxelMesh();
            mesh.Copy(model.mesh);
            mesh.Transform = g.Transform;
            r.mesh = mesh;

            g.Instantiate(name, GameObjectType.ENVIROMENT);

            return g;
        }
    }
}
