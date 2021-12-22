using System.IO;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TextProviders.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> Read(string filename) =>
            filename.AsResult()
                .Then(File.ReadAllText);

        public bool CanRead(string extension) => extension == "txt";
    }
}