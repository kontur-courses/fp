using System;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.Input;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Output;
using TagsCloudContainer.Processing;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Ui;

namespace TagsCloudContainer
{
    public class Transformer
    {
        private readonly IFileReader fileReader;
        private readonly IParser parser;
        private readonly IWordLayout layout;
        private readonly IDrawer drawer;
        private readonly IWriter writer;

        private readonly ImageSettings settings;

        public Transformer(IFileReader fileReader, IWriter writer, IDrawer drawer, IParser parser, IWordLayout layout, ImageSettings settings)
        {
            this.fileReader = fileReader;
            this.writer = writer;
            this.drawer = drawer;
            this.parser = parser;
            this.layout = layout;
            this.settings = settings;
        }

        public void TransformWords(Options options)
        {
            Console.WriteLine($"Reading from: {options.TextFile}");
            Console.WriteLine($"Writing to: {options.ImageFile}");

            var text = fileReader.Read(options.TextFile);
            var parsedWords = parser.ParseWords(text);

            layout.PlaceWords(parsedWords);
            var bitmap = drawer.Draw(layout, settings);

            writer.WriteToFile(bitmap, options.ImageFile);
        }
    }
}