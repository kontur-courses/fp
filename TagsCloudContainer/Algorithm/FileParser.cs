using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public class FileParser : IFileParser
    {
        public Result<List<string>> ReadWordsInFile(string filePath)
        {
            if (!File.Exists(filePath)) 
                return Result.Fail<List<string>>($"The file with the path {filePath} was not found");

            var words = new List<string>();
            var lines = File.ReadAllLines(filePath);

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
