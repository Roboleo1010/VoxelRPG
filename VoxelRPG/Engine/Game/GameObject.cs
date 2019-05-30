using System;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Game;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Game
{
    public class GameObject
    {
        public string Name;
        public GameObjectType Type;

        Component[] components;
        bool IsActive = true;

        #region Component Propertys
        public Transform Transform;
        #endregion

        public GameObject()
        {
            Name = string.Empty;
            components = new Component[Enum.GetNames(typeof(ComponentType)).Length];
            Transform = (Transform)AddComponent<Transform>(ComponentType.Transform);
        }

        public void SetActive(bool newState)
        {
            IsActive = newState;
        }

        public override string ToString()
        {
            return Name;
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
            int index = (int)type;

            components[index] = (Component)Activator.CreateInstance(typeof(T)); //TODO: Create by string
            components[index].Parent = this;

            return components[index];
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
