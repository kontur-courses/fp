using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace TagsCloudVisualizationDI
{
    public static class Checker
    {
        public static void CheckPathToFile(string pathToFile)
        {
            Result.OnFalse(File.Exists(pathToFile),$"pathToFile {pathToFile}  is not exist", (error) => PrintAboutFail(error));
        }


        public static void CheckPathToDirectory(string pathToDirectory)
        {
            Result.OnFalse(Directory.Exists(pathToDirectory), $"pathToDirectory {pathToDirectory}  is not exist", (error) => PrintAboutFail(error));
        }

        private static void PrintAboutFail(string message)
        {
            throw new Exception(message);
        }
    }
}
