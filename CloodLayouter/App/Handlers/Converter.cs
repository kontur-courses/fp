using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using CloodLayouter.Infrastructer;
using ResultOf;

namespace CloodLayouter.App
{
    public class FromWordToTagConverter : IConverter<IEnumerable<Result<string>>, IEnumerable<Result<Tag>>>
    {
        private readonly ImageSettings imageSettings;

        public FromWordToTagConverter(ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;
        }


        public IEnumerable<Result<Tag>> Convert(IEnumerable<Result<string>> data)
        {
            var i = 0;
            var sucsessfull = data
                .Where(x => x.IsSuccess)
                .Select(x => x.GetValueOrThrow())
                .GroupBy(word => word)
                .OrderByDescending(g => g.Count())
                .Reverse()
                .Select(kvp =>
                {
                    var font = new Font(FontFamily.GenericSerif, 28 + 3 * i, FontStyle.Italic);
                    i++;
                    return Result.Of(() => new Tag
                    {
                        Font = font,
                        Size = Graphics.FromImage(new Bitmap(imageSettings.Width, imageSettings.Height))
                            .MeasureString(kvp.Key, font).ToSizeI(),
                        Word = kvp.Key
                    });
                });
            var failed = data.Where(x => !x.IsSuccess)
                .Select(x => Result.Fail<Tag>("Can'c convert tags becouse -> " + x.Error));
            foreach (var suc in sucsessfull)
                yield return suc;
            foreach (var fail in failed)
                yield return fail;
           
        }
    }
}