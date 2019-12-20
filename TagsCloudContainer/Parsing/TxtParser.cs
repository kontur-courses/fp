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
                return ResultExtensions.Fail<string[]>("Cannot read file" + filePath);
            }

            using (var reader = new StreamReader(filePath))
            {
                return ResultExtensions.Ok(reader.ReadToEnd().Split('\n'));
            }
        }
    }
}