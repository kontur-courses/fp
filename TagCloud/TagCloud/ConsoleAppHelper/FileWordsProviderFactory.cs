using System.IO;
using TagCloud.ErrorHandling;
using TagCloud.WordsProvider;

namespace TagCloud.ConsoleAppHelper
{
    public static class FileWordsProviderFactory
    {
        public static Result<IWordsProvider> Create(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".txt":
                    return Result.Of<IWordsProvider>(() => new TxtWordsProvider(filePath));
                case ".doc":
                case ".docx":
                    return Result.Of<IWordsProvider>(() => new MicrosoftWordWordsProvider(filePath));
                default:
                    return Result.Fail<IWordsProvider>($"Extension {extension} is not supported");
            }
        }
    }
}