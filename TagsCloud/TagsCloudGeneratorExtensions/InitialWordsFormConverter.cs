using FailuresProcessing;
using MyStemWrapper;
using System;
using System.Linq;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGeneratorExtensions
{
    [Priority(5)]
    [Factorial("InitialWordsFormConverter")]
    public class InitialWordsFormConverter : IWordsConverter
    {
        private readonly MyStem stem;

        public InitialWordsFormConverter() =>
            stem = new MyStem { PathToMyStem = Metadata.PathToMyStem, Parameters = "-nl" };

        public Result<string[]> Execute(string[] input)
        {
            return
                Result.Ok(input.GroupBy(w => w))
                .Then(e =>
                    e.Select(group =>
                        (stem.Analysis(group.Key)
                            .Split(new[] { "\r\n", "\n", "?", "|" }, StringSplitOptions.None)
                            .First(),
                        group))
                    .ToArray())
                .RefineError($"Path to {typeof(MyStem).Name} is \'{stem.PathToMyStem}\'")
                .Then(e => 
                    Result.Ok(
                        e
                        .SelectMany(d => d.group.Select(s => d.Item1))
                        .ToArray()))
                .RefineError($"{nameof(InitialWordsFormConverter)} failure");
        }
    }
}