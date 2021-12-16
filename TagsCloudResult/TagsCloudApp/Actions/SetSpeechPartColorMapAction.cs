using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetSpeechPartColorMapAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IEnumParser enumParser;
        private readonly IArgbColorParser colorParser;
        private readonly IKeyValueParser keyValueParser;
        private readonly ISpeechPartColorMapSettings settings;

        public SetSpeechPartColorMapAction(
            IRenderArgs renderArgs,
            IEnumParser enumParser,
            IArgbColorParser colorParser,
            ISpeechPartColorMapSettings settings,
            IKeyValueParser keyValueParser)
        {
            this.renderArgs = renderArgs;
            this.enumParser = enumParser;
            this.colorParser = colorParser;
            this.settings = settings;
            this.keyValueParser = keyValueParser;
        }


        public Result<None> Perform()
        {
            return keyValueParser.Parse(renderArgs.SpeechPartColorMap)
                .Select(keyValue =>
                {
                    var speechPart = enumParser.Parse<SpeechPart>(keyValue.Key);
                    var color = colorParser.Parse(keyValue.Value);
                    return Result.Of(() => new KeyValuePair<SpeechPart, Color>(
                        speechPart.GetValueOrThrow(),
                        color.GetValueOrThrow()));
                })
                .CombineResults()
                .Then(keyValues =>
                {
                    settings.ColorMap = new Dictionary<SpeechPart, Color>(keyValues);
                });
        }
    }
}