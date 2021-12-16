using System.Collections.Generic;
using System.Linq;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Results;

namespace TagsCloudApp.WordsLoading
{
    public class FileWordsProvider : IWordsProvider
    {
        private readonly IWordsParser wordParser;
        private readonly IFileTextLoaderResolver loaderResolver;
        private readonly IRenderArgs renderArgs;
        private readonly IEnumParser enumParser;

        public FileWordsProvider(
            IWordsParser wordParser,
            IFileTextLoaderResolver loaderResolver,
            IRenderArgs renderArgs,
            IEnumParser enumParser)
        {
            this.wordParser = wordParser;
            this.loaderResolver = loaderResolver;
            this.renderArgs = renderArgs;
            this.enumParser = enumParser;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            var filename = renderArgs.InputPath;
            return GetFileType(filename)
                .Then(loaderResolver.GetFileTextLoader)
                .Then(loader => loader.LoadText(filename))
                .Then(wordParser.ParseText);
        }

        private Result<FileType> GetFileType(string filename)
        {
            var fileExtension = filename.Split('.').Last();
            return enumParser.Parse<FileType>(fileExtension);
        }
    }
}