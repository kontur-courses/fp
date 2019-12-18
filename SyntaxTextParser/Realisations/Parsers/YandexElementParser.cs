using SyntaxTextParser.Architecture;
using SyntaxTextParser.YandexParser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SyntaxTextParser
{
    public class YandexElementParser : ElementParserWithRules
    {
        private readonly YandexToolUser toolUser;

        private static readonly Regex AnalysisRegex = new Regex(@"^\w+{(\w+)\??=(\w+)[,=]", RegexOptions.Compiled);

        public YandexElementParser(IEnumerable<IElementValidator> elementValidators,
            IElementFormatter elementFormatter, YandexToolUser toolUser) :
            base(elementValidators, elementFormatter)
        {
            this.toolUser = toolUser;
        }

        protected override IEnumerable<TypedTextElement> ParseText(string text)
        {
            var parsedText = toolUser.ParseTextInTool(text);

            var elements = new List<TypedTextElement>();
            foreach (var analysis in parsedText)
            {
                var match = AnalysisRegex.Match(analysis);
                var initialForm = match.Groups[1].Value;
                var partOfSpeech = match.Groups[2].Value;
                if(string.IsNullOrEmpty(initialForm) 
                   || string.IsNullOrEmpty(partOfSpeech)) continue;

                yield return new TypedTextElement(initialForm, partOfSpeech, ElementFormatter);
            }
        }
    }
}