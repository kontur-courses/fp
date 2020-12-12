using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer
{
    class FileReaderTxt : IFileReader
    {
        public string Format { get; set; }

        public FileReaderTxt()
        {
            Format = "txt";
        }

        public Result<IEnumerable<string>> ReadAllLines(string filePath)
        {
            return Result.Of(() => (IEnumerable<string>)File.ReadAllLines(filePath), typeof(FileReaderTxt).Name)
                    .RefineError("Не удалось прочитать слова из файла");
        }
    }
}
