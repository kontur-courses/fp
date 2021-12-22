using FunctionalProgrammingInfrastructure;

namespace CTV.Common
{
    public interface IWordsParser
    {
        public Result<string[]> Parse(string fullString);
    }
}