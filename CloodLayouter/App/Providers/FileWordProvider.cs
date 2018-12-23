using System.Collections.Generic;
using System.IO;
using CloodLayouter.Infrastructer;
using ResultOf;

namespace CloodLayouter.App
{
    public class FileWordProvider : IProvider<IEnumerable<Result<string>>>
    {
        private readonly string[] fileNames;

        public FileWordProvider(string[] fileNames)
        {
            this.fileNames = fileNames;
        }

        public IEnumerable<Result<string>> Get()
        {
            return Read();
        }

        private IEnumerable<Result<string>> Read()
        {
            foreach (var fileName in fileNames)
            {
                var fileRes = Result.Of(() => new StreamReader(fileName));
                
                if (!fileRes.IsSuccess)
                    yield return Result.Fail<string>($"Can't open file {fileName}.");
                else
                    foreach (var line in fileRes.GetValueOrThrow().ReadLines())
                        yield return Result.Ok(line);

            }
        }
    }
}