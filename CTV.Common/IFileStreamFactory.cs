using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common
{
    public interface IFileStreamFactory
    {
        public Result<Stream> OpenOnWriting(string path);
        public Result<Stream> OpenOnReading(string path);
    }
}