using System.Collections.Generic;
using Newtonsoft.Json;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public class JsonParser : IParser
    {
        public Result<IEnumerable<string>> ParseText(string text)
        {
            return Result.Of(() => 
                JsonConvert.DeserializeObject<string[]>(text) as IEnumerable<string>);
        }

        public string GetModeName()
        {
            return "json";
        }
    }
}
