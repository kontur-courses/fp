using System;
using System.IO;
using TagCloudContainer.Result;

namespace TagCloudContainer.FileReaders
{
    public class TxtReader : IFileReader
    {
        public Result<string[]> FileToWordsArray(string filePath)
        {
            return !File.Exists(filePath)
                ? new Result<string[]>("File doesn't exist")
                : new Result<string[]>(null,
                    File.ReadAllText(filePath).Split(new[] {Environment.NewLine, " "}, StringSplitOptions.None));
        }
    }
}