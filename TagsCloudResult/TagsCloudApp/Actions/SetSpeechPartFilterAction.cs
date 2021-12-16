using System.Linq;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetSpeechPartFilterAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly ISpeechPartFilterSettings settings;
        private readonly IEnumParser enumParser;

        public SetSpeechPartFilterAction(
            IRenderArgs renderArgs,
            ISpeechPartFilterSettings settings,
            IEnumParser enumParser)
        {
            this.renderArgs = renderArgs;
            this.settings = settings;
            this.enumParser = enumParser;
        }

        public Result<None> Perform()
        {
            return renderArgs.IgnoredSpeechParts
                .Select(s => enumParser.Parse<SpeechPart>(s))
                .CombineResults()
                .Then(ignoreSpeechParts =>
                {
                    settings.SpeechPartsToRemove = ignoreSpeechParts;
                });
        }
    }
}