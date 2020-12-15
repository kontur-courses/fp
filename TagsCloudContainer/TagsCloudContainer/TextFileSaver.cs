using System;
using System.IO;
using System.Text.RegularExpressions;
using ResultOf;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class TextFileSaver : ITextSaver
    {
        private readonly string filePath;

        public TextFileSaver(string filePath)
        {
            Result.Ok(filePath)
                .Then(ValidateDirectoryPath)
                .Then(ValidateFileName)
                .OnFail(e => throw new ArgumentException(e, nameof(filePath)));

            this.filePath = filePath;
        }

        public void Save(string text)
        {
            File.WriteAllText(filePath, text);
        }

        private Result<string> ValidateDirectoryPath(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            return Validate(path, x => !Directory.Exists(directoryName), $"Directory {directoryName} doesnt exists");
        }

        private Result<string> ValidateFileName(string path)
        {
            var fileName = Path.GetFileName(path);
            var containsBadCharacter =
                Regex.Match(fileName, "[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");
            var extension = Path.GetExtension(fileName);
            return Validate(path, x => containsBadCharacter.Success || extension == "", $"Wrong file name: {fileName}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}