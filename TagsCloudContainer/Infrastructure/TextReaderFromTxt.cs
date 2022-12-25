using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ResultOf;

namespace TagsCloudContainer.Infrastructure
{
    public class TextReaderFromTxt : ITextReader
    {
        public string Filter => "Изображения (*.txt)|*.txt";

        public Result<string> ReadText(string path)
        {
            return Result.Of(() => File.ReadAllText(path));
        }
    }
}
