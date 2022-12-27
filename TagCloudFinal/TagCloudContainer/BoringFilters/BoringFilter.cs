using MystemHandler;
using System.Net;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Result;

namespace TagCloudContainer.BoringFilters
{
    public class BoringFilter : IBoringWordsFilter
    {
        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            var downloadResult = DownloadRuntimeIfNotExist();

            if (!downloadResult.IsSuccess)
                Result.Result.Fail<IEnumerable<string>>(downloadResult.Error);

            try
            {
                MystemMultiThread mystem = new(1, @"mystem.exe");
                return Result.Result.Ok(words
                    .SelectMany(s => mystem.StemWords(s)!)
                    .Where(l => !l.IsSlug)
                    .Select(l => l.Lemma));
            }
            catch(Exception e)
            {
                return Result.Result.Fail<IEnumerable<string>>(e.Message);
            }
        }

        private Result<None> DownloadRuntimeIfNotExist()
        {
            try
            {
                if (!File.Exists("mystem.exe"))
                    using (var wc = new WebClient())
                        wc.DownloadFile(@"https://vk.com/s/v1/doc/845vqKJTZoySMh9De3XCG5-LUbfLsTAqSGKBVvhmOdIvSzSPu7c", "mystem.exe");
                
                return Result.Result.Ok(new None());
            }
            catch (Exception e)
            {
                return Result.Result.Fail<None>($"Ошибка при загрузке mystem: {e.Message}");
            }
        }
    }
}
