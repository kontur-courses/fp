using TagsCloudResult.TagCloud;
using TagsCloudResult.UI;
using TagsCloudResult.Utility;

namespace TagsCloudResult;

public class Application(
    TagCloudVisualizer visualizer,
    IUI ui,
    WordHandler wordHandler,
    WordDataSet wordDataSet,
    ITextHandler textHandler)
{
    public void Run(ApplicationArguments args)
    {
        var textResult = textHandler.ReadText(args.Input);
        if (textResult.IsErr)
        {
            Console.WriteLine(textResult.UnwrapErr());
            return;
        }
        
        var boringResult = textHandler.ReadText(args.Exclude);
        if (boringResult.IsErr)
        {
            Console.WriteLine(boringResult.UnwrapErr());
            return;
        }
        
        var freqDict = wordDataSet.CreateFrequencyDict(textResult.Unwrap());
        freqDict = wordHandler.Preprocessing(
            freqDict, boringResult.Unwrap(), w => w.Length > 3
        );

        var visualizeResult = visualizer.GenerateTagCloud(freqDict);
        if (visualizeResult.IsErr)
        {
            Console.WriteLine(visualizeResult.UnwrapErr());
            return;
        }
        ui.View(args.Output  + "." + args.Format);
    }
}