using TagCloud.ResultMonade;

namespace TagCloud.WordConverter
{
    public class ToInitialFormConverter : IWordConverter
    {
        public Result<string> Convert(string word)
        {
            return Result.Of(() => word)
                    .RefineError($"Word <{word}> can't be converted to intial form");
        }
    }
}
