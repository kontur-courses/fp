using ResultOf;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer
{
    public class RecipientOfWords
    {
        public static Result<IEnumerable<string>> ReadLineByLine(string pathToFile)
        {
            return Result.Ok(pathToFile).Then(path => new StreamReader(path))
                .Then(reader => Enumeration.RepeatUntilNull(reader.ReadLine));
        }
    }
}
