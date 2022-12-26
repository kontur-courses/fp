using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.Drawer.Interfaces
{
    public interface IRectangleLayout
    {
        public void DrawLayout();

        public void SaveLayout();

        public Result<None> PlaceWords(Dictionary<string, int> words);
    }
}