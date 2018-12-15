using System.Linq;
using Result;

namespace TagCloudCreation
{
    public class Formatter : IWordPreparer
    {
        public Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options)
        {
            var preparedWord = string.Join("", word.Trim()
                                                   .Where(char.IsLetter));
            return word == string.Empty ? Maybe<string>.None : preparedWord;
        }
    }
}
