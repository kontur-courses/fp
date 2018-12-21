using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudResult;

namespace TagsCloudContainer.WordsFilter.BoringWords
{
    public class BoringWords : IBoringWords
    {
        private readonly string fileName;

        public BoringWords(string fileName)
        {
            this.fileName = fileName;
        }

        public Result<HashSet<string>> GetBoringWords
        {
            get
            {
                if (fileName == "")
                    return Result.Ok<HashSet<string>>(new HashSet<string>());

                if (!File.Exists(fileName))
                    return Result.Fail<HashSet<string>>(
                        $"Something went wrong. Check the correctness of {fileName} path.");

                var words = File.ReadAllLines(fileName);

                return new HashSet<string>(words.Select(word => word.ToLower())).AsResult();
            }
        }
    }
}
