using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Xml.XPath;
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

            var fontF = new FontFamily(options.FontName);
            var font = new Font(fontF, options.FontSize);
            
            return reader.ReadFile(options.Path)
                .Then(input => wordReader.GetUniqueLowercaseWords(input))
                .Then(lines => lines
                    .Select(x => preprocessor.PreprocessString(x))
                    .Where(x => x != "")
                    .Select(x => new ColoredSizedWord(x, font)))
                .Then(x => Tuple.Create(x.Select(word => layouter.PutNewRectangle(sizeConverter.Convert(word.Word, word.Font))), x))
                .Then(x => colored.AddColoredWordsFromCloudLayouter(x.Item2.ToArray(), layouter, coloringAlgorithm))
                .Then(x => converterToImage.GetImage(colored, options.X, options.Y))
                .Then(image => fileGenerator.GenerateFile(options.OutputName, formatter, image));


            //var input = reader.ReadFile(options.Path);
            //var lines = wordReader.GetUniqueLowercaseWords(input);
            //var words = lines
            //    .Select(x => preprocessor.PreprocessString(x))
            //    .Where(x => x != "")
            //    .Select(x => new ColoredSizedWord(x, font))
            //    .ToArray();

            //var rectangles = Words.Select(x => sizeConverter.Convert(x.Word, x.Font)).ToArray();
            //foreach (var size in rectangles)
            //{
            //    layouter.PutNewRectangle(size);
            //}

            
            //colored.AddColoredWordsFromCloudLayouter(Words, layouter, coloringAlgorithm);
            //var image = converterToImage.GetImage(colored, options.X, options.Y);
            //fileGenerator.GenerateFile(options.OutputName, formatter, image);
            //return Result.Ok();
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
