using TagCloudContainer.Interfaces;
using TagCloudContainer.Result;

namespace TagCloudContainer.Formatters
{
    public class WordFormatter : IWordFormatter
    {
        public Result<IEnumerable<string>> Normalize(
            IEnumerable<string> textWords,
            Func<string, string> normalizeFunction)
        {
            try
            {
                return Result.Result.Ok(textWords.Select(normalizeFunction));
            }
            catch (Exception e)
            {
                return Result.Result.Fail<IEnumerable<string>>($"Ошибка при нормализации: {e.Message}");
            }
        }
    }
}
