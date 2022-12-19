using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class Recognizer : IRecognizer<string>
    {
        public Result<string> Recognize(string value)
        {
            var result = Result<string>.Ok(value);
            return result;
        }
    }
}