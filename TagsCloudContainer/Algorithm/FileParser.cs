using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public class FileParser : IFileParser
    {
        public Result<List<string>> ReadWordsInFile(string filePath)
        {
            if (!File.Exists(filePath)) 
                return Result.Fail<List<string>>($"The file with the path {filePath} was not found");

            try
            {
                var lines = File.ReadAllLines(filePath);
                return ReadWordsInLines(lines, filePath);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<string>>($"An error occurred while reading the file with the path {filePath}: {ex.Message}");
            }
        }

        private Result<List<string>> ReadWordsInLines(string[] lines,  string filePath)
        {
            var words = new List<string>();

            foreach (var line in lines)
            {
                var lineWords = line.ToLower().Trim().Split(' ');
                if (lineWords.Length > 1)
                    return Result.Fail<List<string>>($"The file with the path {filePath} has incorrect content:" +
                        $" more than one word in the line.");
                words.Add(lineWords[0]);
            }

            return Result.Ok(words);
        }
    }
}
