using ResultOf;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagCloud2.Image;
using TagCloud2.TextGeometry;
using TagCloudVisualisation;

namespace TagCloud2
{
    public class InnerCoreLogic
    {
        private readonly IFileReader reader;
        private readonly IWordReader wordReader;
        private readonly IStringPreprocessor preprocessor;
        private readonly IStringToSizeConverter sizeConverter;
        private readonly ICloudLayouter layouter;
        private readonly IColoredCloud coloredCloud;
        private readonly IColoringAlgorithm coloringAlgorithm;
        private readonly IColoredCloudToImageConverter converterToImage;
        private readonly IFileGenerator fileGenerator;
        private readonly IImageFormatter formatter;

        public Result<None> Run(IOptions options)
        {
            var colored = new ColoredCloud();
            var fontCollection = new InstalledFontCollection();
            if (!fontCollection.Families.Select(x => x.Name).Contains(options.FontName))
            {
                return Result.Fail<None>("No such font!");
            }

            if (options.FontSize <= 0)
            {
                return Result.Fail<None>("FontSize is zero or negative");
            }

            var fontF = new FontFamily(options.FontName);
            var font = new Font(fontF, options.FontSize);
            return reader.ReadFile(options.Path)
                .Then(input => wordReader.GetUniqueLowercaseWords(input))
                .Then(lines => lines
                    .Select(x => preprocessor.PreprocessString(x))
                    .Where(x => x != "")
                    .Select(x => new ColoredSizedWord(x, font)))
                .Then(words => Tuple.Create(words.Select(word => layouter.PutNewRectangle(sizeConverter.Convert(word.Word, word.Font))), words))
                .Then(tuple => colored.AddColoredWordsFromCloudLayouter(tuple.Item2.ToArray(), tuple.Item1.ToList(), coloringAlgorithm))
                .Then(x => converterToImage.GetImage(colored, options.X, options.Y))
                .Then(image => fileGenerator.GenerateFile(options.OutputName, formatter, image));
        }

        public InnerCoreLogic(IFileReader reader, IWordReader wordReader, IStringPreprocessor preprocessor, IStringToSizeConverter sizeConverter,
            ICloudLayouter layouter, IColoredCloud coloredCloud, IColoringAlgorithm coloringAlgorithm, 
            IColoredCloudToImageConverter converterToImage, IFileGenerator fileGenerator, IImageFormatter formatter)
        {
            this.reader = reader;
            this.wordReader = wordReader;
            this.preprocessor = preprocessor;
            this.sizeConverter = sizeConverter;
            this.layouter = layouter;
            this.coloredCloud = coloredCloud;
            this.coloringAlgorithm = coloringAlgorithm;
            this.converterToImage = converterToImage;
            this.fileGenerator = fileGenerator;
            this.formatter = formatter;
        }
    }
}
