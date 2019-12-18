using System.Collections.Generic;
using System.IO;
using ResultOf;
using Xceed.Words.NET;

namespace TagsCloudContainer.Core.Readers
{
    class DocReader : IReader
    {
        public Result<IEnumerable<string>> ReadWords(string path)
        {
            return File.Exists(path)
                ? DocX.Load(path).Text.Split()
                : Result.Fail<IEnumerable<string>>($"Файла {path} не существует");

        }

        public bool CanRead(string path)
        {
            var extension = Path.GetExtension(path);
            return extension == ".doc" || extension == ".docx";
        }
    }
}