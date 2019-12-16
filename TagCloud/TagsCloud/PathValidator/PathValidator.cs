using System.IO;
using TagsCloud.ErrorHandling;

namespace TagsCloud.PathValidators
{
    public class PathValidator
    {
        public Result<bool> IsValidPath(string path) => path == null ? Result.Fail<bool>("The path must not be null.") : File.Exists(path).AsResult();
    }
}
