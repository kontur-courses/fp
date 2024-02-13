using SixLabors.Fonts;
using TagsCloudResult.UI;

namespace TagsCloudResult;

public class FontFinder
{
    public static MyResult<FontFamily> TryGetFont(ApplicationArguments args)
    {
        var res =
            MyResult.Try(() => new FontCollection().Add(args.FontPath));
        if (res.IsErr)
            res.ReplaceError($"Font {args.FontPath} doesn't exist");
        return res;
    }
}