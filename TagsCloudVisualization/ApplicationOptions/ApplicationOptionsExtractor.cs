using CommandLine;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.ApplicationOptions
{
    public class ApplicationOptionsExtractor
    {
        public Result<ApplicationOptions> GetOptions(ParserResult<ApplicationOptions> parsedOptions)
        {
            var applicationOptions = new ApplicationOptions();
            var result = Result.OfAction(() => parsedOptions.WithParsed(options => applicationOptions = options));
                
            return result.IsSuccess
                ? applicationOptions.AsResult()
                : Result.Fail<ApplicationOptions>("Can't get options");
            
            /*var applicationOptions = new ApplicationOptions();
            var result = Result.OfAction(() => parsedOptions.WithParsed(options => applicationOptions = options));

            return result.IsSuccess
                ? applicationOptions.AsResult()
                : Result.Fail<ApplicationOptions>("Can't get options");*/
        }
    }
}