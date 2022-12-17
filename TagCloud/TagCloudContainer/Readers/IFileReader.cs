namespace TagCloudContainer.Readers
{
    public interface IFileReader
    {
        Result<string> TxtRead(string path);
        Result<string> DocRead(string path);
    }
}
