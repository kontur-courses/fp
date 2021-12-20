using System.IO;

namespace CTV.Common
{
    public interface IFileStreamFactory
    {
        public Stream OpenOnWriting(string path);
        public Stream OpenOnReading(string path);
    }
}