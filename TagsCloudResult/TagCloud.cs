using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public class TagCloud
    {
        private AppSettings settings;
        
        public TagCloud(AppSettings settings)
        {
            this.settings = settings;
        }

        public void Create(Func<AppSettings, Result<IEnumerable<string>>> read,
            Func<IEnumerable<string>, AppSettings, Result<IEnumerable<LayoutWord>>> select,
            Func<IEnumerable<LayoutWord>, Func<Size, Rectangle>, AppSettings, Result<IEnumerable<(Rectangle, LayoutWord)>>> compositor,
            Func<IEnumerable<(Rectangle, LayoutWord)>, AppSettings, Result<Bitmap>> draw,
            Func<Bitmap, AppSettings, Result<None>> save,
            Func<Size, Rectangle> putNext)
        {
            var result = read(settings).Then(text => select(text, settings))
                .Then(words => compositor(words, putNext, settings))
                .Then(rect => draw(rect, settings))
                .Then(image => save(image, settings));
            if (!result.IsSuccess)
                Console.WriteLine(result.Error);
        }
    }
}