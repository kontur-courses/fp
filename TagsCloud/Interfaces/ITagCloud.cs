namespace TagsCloud.Interfaces
{
    public interface ITagCloud
    {
        public void PrintTagCloud(string textFilePath, string exportFilePath, string extension);
    }
}