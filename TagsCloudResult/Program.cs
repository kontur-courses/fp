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
            args = new string[] { };
            var consoleParser = new ConsoleArgumentParser();
            var path = consoleParser.GetPath(args);
            if (path is null)
                return;
            var imageSetting = consoleParser.GetImageSetting(args);
            var wordSetting = consoleParser.GetWordSetting(args);
            var algSetting = consoleParser.GetAlgorithmsSettings(args);
            var layouter = new CircularCloudLayouter(new Point(imageSetting.Width / 2, imageSetting.Height / 2));
            var result = DrawAndSave(WordReaderFromFile.ReadWords,
                x => BasicWordsSelector.Select(x, wordSetting),
                x => Compositor.Composite(x, layouter, algSetting),
                x => CloudDrawer.Draw(x, imageSetting),
                x => ImageCreator.Save(x, imageSetting),
                path);
            if (result.IsSuccess)
                return;
            Console.Write(result.Error);
        }

        private static Result<None> DrawAndSave(Func<string, Result<IEnumerable<string>>> read,
            Func<IEnumerable<string>, Result<IEnumerable<LayoutWord>>> select,
            Func<IEnumerable<LayoutWord>, Result<IEnumerable<(Rectangle, LayoutWord)>>> compositor,
            Func<IEnumerable<(Rectangle, LayoutWord)>, Result<Bitmap>> draw,
            Func<Bitmap, Result<None>> save,
            string path)
        {
            return read(path).Then(select).Then(compositor).Then(draw).Then(save);
        }
    }
}