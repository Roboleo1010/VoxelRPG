using System.Collections.Generic;
using VoxelRPG.Engine.Game;

namespace VoxelRPG.Engine.Graphics.Rendering
{
    public abstract class RenderBuffer
    {
        public abstract void BindBuffers();
        public abstract void Render();

        public abstract void AddGameObject(GameObject[] o);

        public abstract void RemoveGameObject(GameObject[] o);

        public abstract List<GameObject> GetGameObjects();
    }
}