using Functional;

namespace TagCloudCreation
{
    public interface IWordPreparer
    {
        /// <summary>
        ///     Transforms word
        /// </summary>
        /// <returns>null if this word should be removed</returns>
        Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options);
    }
}
