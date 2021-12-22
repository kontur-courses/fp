using System.IO;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.Text;

namespace TagsCloudVisualization.Default
{
    public class TxtTextReader : ITextReader
    {
        public bool CanReadFile(FileInfo file)
        {
            if (!file.Exists)
                return false;
            var extension = Path.GetExtension(file.Name);
            return extension == ".txt";

        }

        public Result<string> ReadFile(FileInfo file)
        {
            return Result.Of(() => file.OpenText().ReadToEnd(), "Unable to read from file:" + file.Name);
        }
    }
}