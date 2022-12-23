using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.Parsers
{
    public interface IParser
    {
        public string FileType { get; }

        public Result<IEnumerable<string>> WordParse(string path);
    }
}