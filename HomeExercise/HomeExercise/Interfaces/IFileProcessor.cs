using System.Collections.Generic;
using ResultOf;

namespace HomeExercise
{
    public interface IFileProcessor
    {
        //public Dictionary<string, int> GetWords();
        public Result<Dictionary<string, int>> GetWords();
    }
}