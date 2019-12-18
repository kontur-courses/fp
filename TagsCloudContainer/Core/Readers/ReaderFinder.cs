using System.Linq;

namespace TagsCloudContainer.Core.Readers
{
    class ReaderFinder
    {
        private readonly IReader[] readers;
        public ReaderFinder(IReader[] readers)
        {
            this.readers = readers;
        }

        public IReader Find(string filePath) => readers.FirstOrDefault(r => r.CanRead(filePath));
    }
}
