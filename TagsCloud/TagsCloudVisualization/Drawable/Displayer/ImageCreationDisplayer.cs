using System.Collections.Generic;
using ResultMonad;
using TagsCloudDrawer;
using TagsCloudDrawer.ImageCreator;

namespace TagsCloudVisualization.Drawable.Displayer
{
    public class ImageCreationDisplayer : IDrawableDisplayer
    {
        private readonly string _filename;
        private readonly IImageCreator _creator;

        public ImageCreationDisplayer(string filename, IImageCreator creator)
        {
            _filename = filename;
            _creator = creator;
        }

        public Result<None> Display(IEnumerable<Result<IDrawable>> drawables) => _creator.Create(_filename, drawables);
    }
}