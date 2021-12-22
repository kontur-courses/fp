using ResultOf;

namespace TagCloud2.Image
{
    public class FileGenerator : IFileGenerator
    {
        public Result<None> GenerateFile(string name, IImageFormatter formatter, System.Drawing.Image image)
        {
            if (formatter.Codec.IsSuccess)
            {
                return Result.OfAction(() => image.Save(name, formatter.Codec.GetValueOrThrow(), formatter.Parameters));
            }
            else
            {
                return Result.Fail<None>("No such codec");
            }
        }
    }
}
