using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult.Infrastructure.Common;
using TagsCloudResult.Layouter;

namespace TagsCloudResult
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var consoleParser = new ConsoleArgumentParser();
            var settings = consoleParser.GetSettings(args);
            if (settings is null)
                return;
            var layouter = new CircularCloudLayouter();
            var tagCloud = new TagCloud(settings);
            tagCloud.Create(WordReaderFromFile.ReadWords,
                BasicWordsSelector.Select,
                Compositor.Composite,
                CloudDrawer.Draw,
                ImageCreator.Save,
                layouter.PutNextRectangle);
        }
    }
}