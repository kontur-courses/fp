using TagCloud.ResultMonade;

namespace TagCloud.WordFilter
{
    public class BoringWordFilter : IWordFilter
    {
        public Result<bool> IsPermitted(string word)
        {
            return word.Length > 3;
        }
    }
}
