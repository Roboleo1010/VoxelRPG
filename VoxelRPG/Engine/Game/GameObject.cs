using OpenTK;
using VoxelRPG.Engine.Graphics.Meshes;
using VoxelRPG.Game;

namespace VoxelRPG.Engine.Game
{
    public abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        bool IsActive = true;
        string name = string.Empty;

        public virtual void OnUpdate(float deltaTime)
        { }

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

        public void SetActive(bool newState)
        {
            IsActive = newState;
        }

        public override string ToString()
        {
            return name;
        }


    }
}
