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
        var text = textHandler.ReadText(args.Input);
        var freqDict = wordDataSet.CreateFrequencyDict(text);
        freqDict = wordHandler.Preprocessing(
            freqDict, textHandler.ReadText(args.Exclude), w => w.Length > 3
        );

        visualizer.GenerateTagCloud(freqDict);
        ui.View(args.Output  + "." + args.Format);
    }
}