using TagCloud.ResultMonade;

namespace TagCloud.WordConverter
{
    public class ToLowerConverter : IWordConverter
    {
        public Result<string> Convert(string word)
        {
            return Result.Of(() => word.ToLower())
                .RefineError($"Word <{word}> can't be converted to lower case");
        }
    }
}
