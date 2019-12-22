using System.Collections.Generic;
using System.IO;
using ResultOf;

namespace TagsCloudContainer.Reader
{
    public class ReaderFromTxt : IReaderFromFile
    {
        public Result<IEnumerable<string>> GetWordsSet(string path)
        {
            return path.AsResult()
                .Then(GetLine)
                .RefineError("Ошибка, невозможно прочитать из файла");
        }
        private IEnumerable<string> GetLine(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                string strLine;
                while ((strLine = streamReader.ReadLine()) != null)
                    yield return strLine.Trim();
            }
        }
    }
}