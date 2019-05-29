using System;
using System.Reflection;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Game
{
    public class GameObject
    {
        Component[] components;
        bool IsActive = true;
        string name = string.Empty;

        #region Component Propertys
        public Transform Transform { get { return (Transform)GetComponent(ComponentType.Transform); } }
        #endregion

        public GameObject()
        {
            components = new Component[Enum.GetNames(typeof(ComponentType)).Length];
            AddComponent<Transform>(ComponentType.Transform);
        }

        public void SetActive(bool newState)
        {
            IsActive = newState;
        }

        public override string ToString()
        {
            return name;
        }

        #region Virtual OnUpdate 
        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < components.Length; i++)
                if (components[i] != null)
                    components[i].OnUpdate(deltaTime);

            OnUpdateVirtual(deltaTime);
        }

        protected virtual void OnUpdateVirtual(float deltaTime)
        {

        }
        #endregion

        #region Virtual Destroy
        public void Destroy()
        {
            DestroyVirtual();

            GameManager.window.RemoveGameObject(this);
        }

        protected virtual void DestroyVirtual()
        {

        }
        #endregion

        #region Components
        public Component AddComponent<T>(ComponentType type)
        {
            components[(int)type] = (Component)Activator.CreateInstance(typeof(T)); //TODO: Create by string
            return components[(int)type];
        }

        public Component GetComponent(ComponentType type)
        {
            return components[(int)type];
        }

        public void RemoveComponent(ComponentType type)
        {
            components[(int)type] = null;
        }
        #endregion
    }
}
