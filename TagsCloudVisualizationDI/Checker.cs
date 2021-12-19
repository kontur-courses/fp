using System.IO;

namespace TagsCloudVisualizationDI
{
    public static class Checker
    {
        public static void CheckPathToFile(string pathToFile)
        {
            Result.OnFalse(File.Exists(pathToFile),(error) 
                => Program.PrintAboutFail(error), $"pathToFile {pathToFile}  is not exist");
        }


        public static void CheckPathToDirectory(string pathToDirectory)
        {
            Result.OnFalse(Directory.Exists(pathToDirectory), (error) 
                => Program.PrintAboutFail(error), $"pathToDirectory {pathToDirectory}  is not exist");
        }
    }
}
