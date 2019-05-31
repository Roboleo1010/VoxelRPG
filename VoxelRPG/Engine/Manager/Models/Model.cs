using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using VoxelRPG.Game.Enviroment;
using VoxelRPG.Game.Graphics.Meshes;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Manager.Models
{
    public class Model
    {
        public string name;
        public float scale;
        public Vector3Int gridSize;
        [JsonProperty("colors")]
        public List<ModelColor> modelColors;
        [JsonProperty("voxels")]
        public List<ModelVoxel> modelVoxels;

        public VoxelMesh mesh;

        public void Build()
        {
            //Voxel[,,] voxels = new Voxel[gridSize.X, gridSize.Y, gridSize.Z]; TODO
            //
            ////Update Data
            //foreach (ModelVoxel v in modelVoxels)
            //{
            //    v.color = modelColors.Where(x => x.name == v.colorName).FirstOrDefault().color;
            //    v.colorName = string.Empty;
            //
            //    voxels[v.position.X, v.position.Y, v.position.Z] = new Voxel(new Vector3Int(v.position.X, v.position.Y, v.position.Z), v.color);
            //}
            //modelColors = null;
            //
            //
            //
            //
            //mesh = new VoxelMesh(voxels);
        }
    }
}
