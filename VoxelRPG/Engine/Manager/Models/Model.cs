using System.Collections.Generic;
using System.Linq;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Manager.Models
{
    public class Model
    {
        public string name;
        public float scale;
        public Vector3Int gridSize;
        public List<ModelColor> colors;
        public List<ModelVoxel> voxels;

        public void Build()
        {
            //Update Data
            foreach (ModelVoxel v in voxels)
            {
                v.color = colors.Where(x => x.name == v.colorName).FirstOrDefault().color;
                v.colorName = string.Empty;
            }
            colors = null;

            //BuildMesh

        }
    }
}
