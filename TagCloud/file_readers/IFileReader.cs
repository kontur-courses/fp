using System.Collections.Generic;

namespace TagCloud.file_readers
{
    public interface IFileReader
    {
        Result<List<string>> GetWords(string filename);
    }
}