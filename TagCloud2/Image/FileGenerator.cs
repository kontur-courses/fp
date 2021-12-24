using ResultOf;

namespace TagCloud2.Image
{
    public class FileGenerator : IFileGenerator
    {
        public Result<None> GenerateFile(string name, IImageFormatter formatter, System.Drawing.Image image)
        {
            return formatter.Codec.IsSuccess
                ? Result.OfAction(() => image.Save(name, formatter.Codec.GetValueOrThrow(), formatter.Parameters))
                : Result.Fail<None>("No such codec");
        }
    }
}
