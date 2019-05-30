using System.IO;

namespace VoxelRPG.Utilitys
{
    public static class FileUtility
    {
        public static string[] GetAllFilesOfType(string path, string filter)
        {
            if (Directory.Exists(path))
                return Directory.GetFiles(path, filter);

            return new string[0];
        }
    }
}
