using System;
using System.IO;

namespace TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile
{
    public class FileExistsСhecker : IFileExistsСhecker
    {
        public string GetProvenPath(string pathToFile)
        {
            if (File.Exists(pathToFile))
                return pathToFile;
            throw new Exception("Doesn't contain the text file");
        }
    }
}