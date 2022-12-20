using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class Recognizer : IRecognizer<string>
    {
        public ResultHandler<string> Recognize(string value)
        {
            var result = new ResultHandler<string>(value);
            return result;
        }
    }
}