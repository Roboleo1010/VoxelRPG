using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VoxelRPG.Engine.Diagnosatics;
using VoxelRPG.Utilitys;

namespace VoxelRPG.Engine.Manager.Models
{
    public static class ModelManager
    {
        public static Dictionary<string, Model> models = new Dictionary<string, Model>();

        public static void Start()
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

        public static Model GetModel(string name)
        {
            Model model;

            if (models.TryGetValue(name, out model))
                return model;

            Debug.LogError("No model with name: " + name + " found");
            return null;
        }
    }
}
