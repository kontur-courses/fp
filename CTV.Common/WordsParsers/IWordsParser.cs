using FunctionalProgrammingInfrastructure;

namespace CTV.Common.WordsParsers
{
    public interface IWordsParser
    {
        public Result<string[]> Parse(string fullString);
    }
}