using System.Collections.Generic;
using TagsCloudContainer;

namespace TagsCloudApp.WordsLoading
{
    public interface IFileTextLoader
    {
        IEnumerable<FileType> SupportedTypes { get; }
        Result<string> LoadText(string filename);
    }
}