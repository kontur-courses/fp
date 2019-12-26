namespace TagsCloudForm.Common
{
    public interface IBlobStorage
    {
        Result<byte[]> Get(string name);
        void Set(string name, byte[] content);
    }
}