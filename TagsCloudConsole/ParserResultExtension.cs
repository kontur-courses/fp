using CommandLine;
using ResultOf;

namespace TagsCloudConsole
{
    public static class ParserResultExtension
    {
        public static Result<T> ToResult<T>(this ParserResult<T> parserResult)
        {
            if (parserResult.Tag == ParserResultType.NotParsed)
                return Result.Fail<T>("");

            T options = default(T);
            parserResult.WithParsed(opts => options = opts);
            return options;
        }
    }
}
