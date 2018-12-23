using System.Collections.Generic;
using System.Linq;
using CloodLayouter.Infrastructer;
using ResultOf;

namespace CloodLayouter.App.Handlers
{
    public class WordSelector : IConverter<IEnumerable<Result<string>>, IEnumerable<Result<string>>>
    {


        public IEnumerable<Result<string>> Convert(IEnumerable<Result<string>> data)
        {
            foreach (var result in data)
            {
                if (result.IsSuccess)
                    yield return result;
                else
                    yield return Result.Fail<string>("Can't select word becous -> " + result.Error);
            }

        }
    }
}