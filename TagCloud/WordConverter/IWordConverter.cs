using TagCloud.ResultMonade;

namespace TagCloud.WordConverter
{
    public interface IWordConverter
    {
        Result<string> Convert(string word);
    }
}
