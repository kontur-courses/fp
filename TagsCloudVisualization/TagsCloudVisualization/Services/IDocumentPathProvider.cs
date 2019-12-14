using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public interface IDocumentPathProvider
    {
        Result<string> GetPath();
        void SetPath(string path);
    }
}