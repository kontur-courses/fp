namespace TagCloudGenerator.TextReaders
{
    public class TxtReader : ITextReader
    {
        public string GetFileExtension() => ".txt";

        public Result<IEnumerable<string>> ReadTextFromFile(string filePath)
        {
            try
            {
                return new Result<IEnumerable<string>>(File.ReadAllLines(filePath), null);
            }
            catch
            {
                return new Result<IEnumerable<string>>(null, $"Could not find file {filePath}");
            }

        }
    }
}