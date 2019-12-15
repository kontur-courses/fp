using System.Collections.Generic;
using System.IO;
using TagCloudContainer.Api;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Implementations
{
    public class TxtFileReader : IWordProvider
    {
        private readonly string txtFileName;

        public TxtFileReader(string txtFileName)
        {
            this.txtFileName = txtFileName;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            if (File.Exists(txtFileName))
            {
                return Result.Of(() => File.ReadLines(txtFileName));
            }

            return Result.Fail<IEnumerable<string>>($"File not found: {txtFileName}");
        }
    }
}