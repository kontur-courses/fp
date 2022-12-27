using MystemHandler;
using System.Net;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Result;

namespace TagCloudContainer.Parsers
{
    public class FileLinesParser : IFileParser
    {
        public Result<IEnumerable<string>> Parse(string text)
        {
            try
            {
                return Result.Result.Ok(text.Split(Environment.NewLine).Select(x => x));
            }
            catch (Exception e)
            {
                return Result.Result.Fail<IEnumerable<string>>($"Ошибка при парсинге линий: {e.Message}");
            }
        }
    }
}
