using System.IO;

namespace TagCloud.Readers
{
    public class TextReader : IFileReader
    {
        public Result<string[]> ReadFile(string filename)
        {
            return File.Exists(filename) 
                ? File.ReadAllLines(filename).AsResult()
                : Result.Fail<string[]>($"File {filename} not found." +
                                        $" Please check that file exists");
        }
    }
}
