using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.PreprocessingWords
{
    public interface ICreateProcess
    {
        IEnumerable<string> GetResult(string nameProgram, string arguments);
    }
}