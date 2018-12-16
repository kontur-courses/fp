using System.Collections.Generic;

namespace TagsCloudContainer.FileReader
{
    public class ReadFileResult
    {
        public IEnumerable<string> ReadData { get; }
        public string ErrorMessage { get; }

        public ReadFileResult(IEnumerable<string> readData, string errorMessage)
        {
            ReadData = readData;
            ErrorMessage = errorMessage;
        }
    }
}