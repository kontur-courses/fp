using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Core.Readers
{
    class ReaderFinder
    {
        private readonly IReader[] readers;

        public ReaderFinder(IReader[] readers)
        {
            this.readers = readers;
        }

        public Result<IReader> Find(string filePath)
        {
            var result = readers.FirstOrDefault(r => r.CanRead(filePath));
            return Result.Ok(result)
                .FailIf(r => r == null,
                    "Формат входного файла не поддерживается");
        }
    }
}