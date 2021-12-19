using System.Diagnostics;
using ResultOf;

namespace TagCloud.Extensions
{
    public static class ProcessExtensions
    {
        public static Result<None> Start(this Process process)
        {
            try
            {
                process.Start();
                return Result.Ok();
            }
            catch
            {
                return Result.Fail<None>("Процесс не запустился");
            }
        }
    }
}