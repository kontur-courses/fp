using System.Collections.Generic;
using TagsCloud.Result;

namespace TagsCloud.Drawer
{
    public interface IRectangleLayout
    {
        public void DrawLayout();

        public void SaveLayout();

        public Result<None> PlaceWords(Dictionary<string, int> words);
    }
}