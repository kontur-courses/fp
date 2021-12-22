using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common
{
    public class FileStreamFactory : IFileStreamFactory
    {
        public Result<Stream> OpenOnWriting(string path)
        {
            return Result
                .Of(() => new FileStream(path, FileMode.OpenOrCreate))
                .Then(stream => stream as Stream);
        }

        public Result<Stream> OpenOnReading(string path)
        {
            return Result
                .Of(() => new FileStream(path, FileMode.Open))
                .Then(fileStream => fileStream as Stream);
        }
    }
}