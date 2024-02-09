namespace TagCloudResult.TextProcessing
{
    public class FileTextReader : ITextReader
    {
        public Result<IEnumerable<string>> GetWordsFrom(string filePath)
        {
            var words = new List<string>();
            if (!File.Exists(filePath))
                return ResultIs.Fail<IEnumerable<string>>(
                    $"Cant't find file with this path {Path.GetFullPath(filePath)}"
                );
            using var sr = new StreamReader(filePath);
            var line = sr.ReadLine();
            while (line != null)
            {
                words.Add(line!.ToLower());
                line = sr.ReadLine();
            }

            return ResultIs.Ok(words as IEnumerable<string>);
        }
    }
}
