using System.IO;

namespace CTV.Common.Readers
{
    public interface IFileReader
    {
        public string ReadToEnd(Stream inputSteam);
    }
}