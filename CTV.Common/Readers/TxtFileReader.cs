using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common.Readers
{
    public class TxtFileReader: IFileReader
    {
        public Result<string> ReadToEnd(Stream inputStream)
        {
            return Result.Of(() => new StreamReader(inputStream))
                .Then(reader => reader.ReadToEnd());
        }
    }
}