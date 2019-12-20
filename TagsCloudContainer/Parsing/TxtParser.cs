using System.Collections.Generic;
using System.IO;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Parsing
{
    public class TxtParser : IFileParser
    {
        public Result<string[]> ParseFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var a = Result.Fail<string>("as");
                return Result.Fail<string[]>("Cannot read file" + filePath);
            }

            using (var reader = new StreamReader(filePath))
            {
                return Result.Ok(reader.ReadToEnd().Split('\n'));
            }
        }
    }
}