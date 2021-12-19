using CommandLine;
using TagsCloudContainer;

namespace TagsCloud.Console
{
    public class ArgumentsParser
    {
        public static Result<T> Parse<T>(string[] args)
            where T : new()
        {
            using var parser = new Parser(config =>
            {
                config.CaseInsensitiveEnumValues = true;
                config.IgnoreUnknownArguments = true;
            });
            var parsed = parser.ParseArguments<T>(args) as Parsed<T>;
            if (parsed == null)
                return Result.Fail<T>("Syntax Error. Could not parse console input");
            
            return parsed.Value;
        }
    }
}