using System.Collections.Generic;
using ResultOf;

namespace HomeExercise
{
    public interface IWordsProcessor
    {
        public Result<List<IWord>> HandleWords(); 
    }
}