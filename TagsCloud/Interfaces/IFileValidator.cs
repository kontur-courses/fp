namespace TagsCloud.Interfaces
{
    public interface IFileValidator
    {
        public Result<string> VerifyFileExistence(string path);
    }
}