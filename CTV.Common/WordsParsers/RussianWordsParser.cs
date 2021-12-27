using System.Linq;
using System.Text.RegularExpressions;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common.WordsParsers
{
    public class RussianWordsParser : IWordsParser
    {
        private static readonly Regex WordPattern = new Regex("[а-яА-ЯёЁ]+");

        public Result<string[]> Parse(string fullString)
        {
            return Result
                .Of(() => WordPattern.Matches(fullString))
                .Then(matches => matches.Select(match => match.Value).ToArray());
        }
    }
}