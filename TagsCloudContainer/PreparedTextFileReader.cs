using System.IO;

namespace TagsCloudContainer
{
    public class PreparedTextFileReader : ISource
    {
        private readonly string filename;

        public PreparedTextFileReader(string filename)
        {
            this.filename = filename;
        }

        public Result<string[]> GetWords()
        {
            if (filename == null)
                return Result.Ok(new string[0]);

            return Result.Of(() => File.ReadAllLines(filename));
        }
    }
}
