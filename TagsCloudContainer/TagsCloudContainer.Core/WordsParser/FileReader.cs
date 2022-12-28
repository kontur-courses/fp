using System.Text.RegularExpressions;
using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.WordsParser.ExtensionReaders;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser
{
    public class FileReader : IFileReader
    {
        private static readonly Regex ExtensionRegex = new(@".*?(?<extension>\.[^.]*?)$");
        private delegate Result<IEnumerable<string>> ReadWordsMethod();
        private IExtensionReader? _fileExtensionReader;
        private readonly string _filePath;

        public FileReader(string filePath)
        {
            _filePath = filePath;
        }

        private Result<IEnumerable<string>> ReadWordsFromFile() => CheckFileExisting().Then(_ => _fileExtensionReader?.ReadWords())!;

        private Result<ReadWordsMethod> GetReadFileMethod(string extension)
        {
            _fileExtensionReader = extension switch
            {
                ".txt" => new TxtReader(_filePath),
                ".docx" => new DocxReader(_filePath),
                ".odt" => new OdtExtensionReader(_filePath),
                _ => null
            };

            return _fileExtensionReader is not null 
                ? Result.Ok<ReadWordsMethod>(ReadWordsFromFile) 
                : Result.Fail<ReadWordsMethod>("Unsupported file extension!");
        }

        public Result<IEnumerable<string>> ReadWords() =>
            GetFileExtension(_filePath)
                .Then(GetReadFileMethod)
                .Then(readWords => readWords.Invoke());

        private Result<bool> CheckFileExisting()
            => File.Exists(_filePath) ? true : Result.Fail<bool>($"File {_filePath} not found.");

        private static Result<string> GetFileExtension(string filePath) 
            => Result.Of(() => ExtensionRegex.Match(filePath).Groups["extension"].Value).ReplaceError(_ => $"Incorrect extension on file {filePath}");

    }
}
