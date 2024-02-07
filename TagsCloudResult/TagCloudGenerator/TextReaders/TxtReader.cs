namespace TagCloudGenerator.TextReaders
{
    public class TxtReader : ITextReader
    {
        public string GetFileExtension() => ".txt";

        public Result<IEnumerable<string>> ReadTextFromFile(string filePath)
        {
            try
            {
                return Result<IEnumerable<string>>.Success(File.ReadAllLines(filePath));
            }
            catch
            {
                return Result<IEnumerable<string>>.Failure($"Could not find file {filePath}");
            }

        }
    }
}