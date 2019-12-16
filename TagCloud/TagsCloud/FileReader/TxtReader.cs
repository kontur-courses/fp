using System;
using System.IO;
using TagsCloud.Interfaces;
using TagsCloud.PathValidators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.FileReader
{
    public class TxtReader : ITextReader
    {
        private readonly PathValidator pathValidator;

        public TxtReader(PathValidator pathValidator)
        {
            this.pathValidator = pathValidator;
        }

        public Result<string> ReadFile(string path)
        {
            if (Path.GetExtension(path) != ".txt")
                return Result.Fail<string>($"Unsupported extension {Path.GetExtension(path)}");
            return pathValidator.IsValidPath(path)
                .Then(fileExists => fileExists ? Result.Of(() => File.ReadAllText(path)) : Result.Fail<string>($"File {path} not exist"));
        }
    }
}
