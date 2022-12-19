using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class Recognizer : IRecognizer<string>
    {
        public Result<string> Recognize(string value)
        {
            var result = new Result<string>(null, value);
            return result;
        }
    }
}