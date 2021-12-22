using ResultOf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloud2.Text
{
    public class ExcludedWordsSelector : ISillyWordSelector
    {
        private HashSet<string> excluded;
        public bool IsWordSilly(string word)
        {
            return excluded.Contains(word);
        }

        public Result<None> SetExcludedWordsSelector(IFileReader fileReader, IWordReader wordReader, ExcludedWordsPath path)
        {
            if (!File.Exists(path.Path))
            {
                return Result.Fail<None>("File for exclude doesn't exists");
            }

            var fileContent = fileReader
                .ReadFile(path.Path)
                .Then(wordReader.GetUniqueLowercaseWords);
            wordReader.GetUniqueLowercaseWords(fileReader.ReadFile(path.Path).GetValueOrThrow()).ToHashSet();
            return Result.Ok();
        }
    }
}
