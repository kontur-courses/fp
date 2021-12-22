using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common.Readers
{
    public class TxtFileReader: IFileReader
    {
        public Result<string> ReadToEnd(Stream inputSteam)
        {
            var textSteam = new StreamReader(inputSteam);
            return textSteam.ReadToEnd();
        }
    }
}