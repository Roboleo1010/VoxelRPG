using OpenTK;
using System;
using VoxelRPG.Engine.Game.Components;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game;
using static VoxelRPG.Constants.Enums;

namespace VoxelRPG.Engine.Game
{
    public abstract class GameObject
    {
        Component[] components;
        bool IsActive = true;
        string name = string.Empty;

        #region Component Propertys
        public Transform Transform { get { return (Transform)GetComponent<Transform>(); } }
        #endregion

        public GameObject()
        {
            components = new Component[Enum.GetNames(typeof(ComponentType)).Length];
            AddComponent<Transform>();
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
                components[i].OnUpdate(deltaTime);

            OnUpdateVirtual(deltaTime);
        }

        protected virtual void OnUpdateVirtual(float deltaTime)
        {

        }
        #endregion

        #region Virtual GetMesh
        public Mesh GetMesh()
        {
            if (IsActive)
                return GetMeshVirtual();
            else
                return null;
        }

        protected virtual Mesh GetMeshVirtual()
        {
            return null;
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
        public void AddComponent<T>()
        {
            components[GetEnumByType<T>()] = (Component)Activator.CreateInstance(typeof(T));
        }

        public Component GetComponent<T>()
        {
            return components[GetEnumByType<T>()];
        }

        int GetEnumByType<T>()
        {
            return (int)Enum.Parse(typeof(ComponentType), typeof(T).Name);
        }
        #endregion
    }
}
