using SixLabors.Fonts;
using TagsCloudResult.UI;

namespace TagsCloudResult;

public class FontFinder
{
    private static MyResult<FontFamily>? _tmp;
    
    public static MyResult<FontFamily> TryGetFont(ApplicationArguments args)
    {
        if (_tmp != null)
            return _tmp;
        
        _tmp = MyResult.Try(() => new FontCollection().Add(args.FontPath));
        if (_tmp.IsErr)
            _tmp.ReplaceError($"Font {args.FontPath} doesn't exist");
        
        return _tmp;
    }
}