using System.Linq;
using FailuresProcessing;
using MyStemWrapper;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGeneratorExtensions
{
    [Priority(5)]
    [Factorial("TakenPartsOfSpeechFilter")]
    public class TakenPartsOfSpeechFilter : IWordsFilter
    {
        private readonly Settings settings;
        private readonly MyStem stem;

        public TakenPartsOfSpeechFilter(Settings settings)
        {
            this.settings = settings;
            stem = new MyStem { PathToMyStem = Metadata.PathToMyStem, Parameters = "-nli" };
        }

        public Result<string[]> Execute(string[] input)
        {
            var takenPartsOfSpeech = 
                settings.TakenPartsOfSpeech
                .Select(p => p.ToUpper())
                .ToArray();
            return
                Result.Ok(input.GroupBy(w => w))
                .Then(e => e.Select(group => (stem.Analysis(group.Key), group)).ToArray())
                .RefineError($"Path to {typeof(MyStem).Name} is \'{stem.PathToMyStem}\'")
                .Then(e =>
                    Result.Ok(
                        e
                        .Where(d =>
                            d.Item1.Split(takenPartsOfSpeech, System.StringSplitOptions.None)
                            .Length > 1)
                        .SelectMany(d => d.group.Select(s => s))
                        .ToArray()))
                .RefineError($"{nameof(TakenPartsOfSpeechFilter)} failure");
        }
    }
}