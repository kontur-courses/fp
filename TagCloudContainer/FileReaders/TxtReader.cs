using System;
using System.IO;

namespace TagCloudContainer.FileReaders
{
    public class TxtReader : IFileReader
    {
        public delegate TxtReader Factory();

        public string[] FileToWordsArray(string filePath)
        {
            return File.ReadAllText(filePath).Split(new[] {Environment.NewLine, " "}, StringSplitOptions.None);
        }
    }
}