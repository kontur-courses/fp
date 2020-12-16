using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.TextProcessing.Readers
{
    public class TxtReader : IReader
    {
        private readonly HashSet<string> supportingExtensions = new HashSet<string>{".txt"};
        
        public Result<string> ReadText(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<string>($"File {path} does not exist");
            if (!CanReadFile(path))
                return Result.Fail<string>($"TxtReader doesn't support extension {Path.GetExtension(path)}");
            
            return File.ReadAllText(path);
        }

        public bool CanReadFile(string path) => supportingExtensions.Contains(Path.GetExtension(path));
    }
}