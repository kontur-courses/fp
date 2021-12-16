using System.Linq;
using TagsCloudApp.Parsers;
using TagsCloudApp.RenderCommand;
using TagsCloudContainer;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Interfaces;

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
                .Select(s => enumParser.TryParse<SpeechPart>(s))
                .CombineResults()
                .Then(ignoreSpeechParts =>
                {
                    settings.SpeechPartsToRemove = ignoreSpeechParts;
                });
        }
    }
}