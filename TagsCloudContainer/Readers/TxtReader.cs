using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.Readers
{
    public class TxtReader : IFileReader
    {
        public Result<IEnumerable<string>> ReadWords(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);

                var words = lines.SelectMany(line => line.Split());

                var nonEmptyWords = words.Where(word => !string.IsNullOrEmpty(word));

                return Result.Ok(nonEmptyWords);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<string>>($"Error reading file: {ex.Message}");
            }
        }
    }
}
