using System.IO;
using CTV.Common;

namespace CTV.Common
{
    public class FileStreamFactory : IFileStreamFactory
    {
        public Stream OpenOnWriting(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate);
        }

        public Stream OpenOnReading(string path)
        {
            return new FileStream(path, FileMode.Open);
        }
    }
}