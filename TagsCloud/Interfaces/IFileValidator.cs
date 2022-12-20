namespace TagsCloud.Interfaces
{
    public interface IFileValidator
    {
        public ResultHandler<string> VerifyFileExistence(string path);
    }
}