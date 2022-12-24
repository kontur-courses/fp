using System;
using System.IO;
using TagCloudContainer.TaskResult;

namespace TagCloudContainer.FileReaders
{
    public class TxtReader : IFileReader
    {
        public Result<string[]> FileToWordsArray(string filePath)
        {
            return !File.Exists(filePath)
                ? Result.OnFail<string[]>("File doesn't exist")
                : Result.OnSuccess(File.ReadAllText(filePath)
                    .Split(new[] {Environment.NewLine, " "}, StringSplitOptions.None));
        }
    }
}