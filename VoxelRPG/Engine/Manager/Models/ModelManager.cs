using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Manager.Models
{
    public static class ModelManager
    {
        private static Dictionary<string, Model> models = new Dictionary<string, Model>();

        public static void Init()
        {
            foreach (string file in FileUtility.GetAllFilesOfType(string.Format("GameData/Models/"), "*.json"))
                LoadFileData(file);
        }

        private static void LoadFileData(string file)
        {
            Model m = JsonConvert.DeserializeObject<Model>(File.ReadAllText(file));
            m.Build();

            models.Add(m.name, m);
        }
    }
}
