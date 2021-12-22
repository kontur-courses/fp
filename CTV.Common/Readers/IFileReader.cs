using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common.Readers
{
    public interface IFileReader
    {
        public Result<string> ReadToEnd(Stream inputSteam);
    }
}