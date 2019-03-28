using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleDockerUI.App.Utils
{
    public class PathUtils
    {
        public static string GetConfigFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), typeof(PathUtils).Assembly.GetName().Name);
        }

        public static string GetConfigFile(string fileName)
        {
            var folderName = GetConfigFolder();
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
            return Path.Combine(folderName, fileName);
        }
    }
}
