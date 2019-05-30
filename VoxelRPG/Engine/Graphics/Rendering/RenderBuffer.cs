using System.Collections.Generic;
using VoxelRPG.Engine.Game;

namespace VoxelRPG.Engine.Graphics.Rendering
{
    public abstract class RenderBuffer
    {
        List<GameObject> gameObjects = new List<GameObject>();
        public bool IsChanged = false;

        public virtual void GatherData() { }
        public virtual void BindBuffers() { }
        public virtual void Render() { }

        public void AddGameObject(GameObject o)
        {
            gameObjects.Add(o);
            IsChanged = true;
        }

        public void RemoveGameObject(GameObject o)
        {
            gameObjects.Remove(o);
            IsChanged = true;
        }

        public List<GameObject> GetGameObjects()
        {
            return gameObjects;
        }
    }
}