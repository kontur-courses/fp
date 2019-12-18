using System.Collections.Generic;
using System.IO;
using ResultOf;

namespace TagsCloudContainer.Core.Readers
{
    class TxtReader : IReader
    {
        public Result<IEnumerable<string>> ReadWords(string path)
        {
            return File.Exists(path) 
                ? File.ReadAllText(path).Split()
                : Result.Fail<IEnumerable<string>>($"Файла {path} не существует");
        } 
        public bool CanRead(string path) => Path.GetExtension(path) == ".txt";
    }
}