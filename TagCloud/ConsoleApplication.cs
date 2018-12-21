using System;
using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public class ConsoleApplication : IApplication
    {

        public Result<None> Run(string input, string output, ITextParcer textParser, 
            ICloudLayouter cloud,
            Visualizer visualizer)
        {
            return textParser.TryGetWordsFromText(input)
                 .Then(textParser.ParseWords)
                .Then(cloud.AddWordsFromDictionary)
                .Then((parsedCloud) => visualizer.RenderCurrentConfig(parsedCloud, output));
        }
    }
}