namespace TagCloud.TextHandlers.Converters
{
    public class ToLowerConverter : IConverter
    {
        public Result<string> Convert(string word)
        {
            return word.AsResult().Then(w => w.ToLower());
        }
    }
}