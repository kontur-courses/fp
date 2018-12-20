namespace TagsCloudContainer.Output
{
    public interface IWriter
    {
        void WriteToFile(byte[] bytes, string name);
    }
}