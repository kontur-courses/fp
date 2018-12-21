using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Processing.Filtering
{
    public class BlackListFilter : IWordFilter
    {
        private readonly HashSet<string> wordsToFilter;

        public BlackListFilter(IEnumerable<string> wordsToFilter)
        {
            this.wordsToFilter = new HashSet<string>(wordsToFilter);
        }

        public IEnumerable<string> Filter(IEnumerable<string> words) =>
            words.Where(word => !wordsToFilter.Contains(word));


        public static Result<BlackListFilter> FromFile(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<BlackListFilter>("File not found");

            if (Path.GetExtension(path) != ".txt")
                return Result.Fail<BlackListFilter>("Bad file format. Use only txt");

            var words = File.ReadLines(path);
            return Result.Ok(new BlackListFilter(words));
        }
    }
}