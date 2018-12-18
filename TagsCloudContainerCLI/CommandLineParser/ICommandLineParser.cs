using System.Collections.Generic;
using TagsCloudContainer.Configuration;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainerCLI.CommandLineParser
{
    public interface ICommandLineParser<TConfiguration> where TConfiguration : IConfiguration
    {
        Result<TConfiguration> Parse(IEnumerable<string> args);
    }
}