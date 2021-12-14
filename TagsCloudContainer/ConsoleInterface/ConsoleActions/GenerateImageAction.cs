using LightInject;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class GenerateImageAction : IUIAction
    {
        private IPreprocessorsApplier preprocessorsApplier;
        IFiltersApplier filtersApplier;
        private ITagPainter tagPainter;
        ServiceContainer container;
        private Dictionary<string, IParser> parserSelector;
        private TagCloudPainter cloudPainter;

        public GenerateImageAction(TagCloudPainter cloudPainter, 
        IFiltersApplier filtersApplier, ServiceContainer container,
        CloudSettings cloudSettings, IParser[] parsers,
        IPreprocessorsApplier preprocessorsApplier)
        {
            this.preprocessorsApplier = preprocessorsApplier;
            this.filtersApplier = filtersApplier;
            tagPainter = cloudSettings.Painter;
            this.container = container;
            this.cloudPainter = cloudPainter;
            parserSelector = new Dictionary<string, IParser>();
            for (var i = 1; i <= parsers.Length; i++)
            {
                var available = parsers[i - 1];
                foreach (var format in available.GetFormats())
                    parserSelector[format] = available;
            }          
        }

        public string GetDescription() => "Generate image";

        public void Handle()
        {
            var textsPath = Path.GetFullPath(@"..\..\..\texts");

            Console.WriteLine($"Texts path is {textsPath}");
            Console.WriteLine("Enter file from this folder as");
            Console.WriteLine("NAME.FORMAT");
            Console.WriteLine("Or pass an empty string to be brought back to menu");
            Console.WriteLine("Supported formats are");
            Console.WriteLine(string.Join(',', parserSelector.Keys.ToArray()));
            var answer = Console.ReadLine() ?? "";
            Console.WriteLine();
            var result = HandleInput(answer);
            if (result.IsSuccess)
                PaintSource(Path.Combine(textsPath, answer), result.Value);
            else
                Console.WriteLine(result.Error);
        }

        private Result<IParser> HandleInput(string answer)
        {
            var split = answer.Split('.');

            if (split.Length != 2)
                return Result.Fail<IParser>("Incorrect input!");

            var format = split[1];
            if (!parserSelector.ContainsKey(format))
                return Result.Fail<IParser>("File format is not supported!");
            return Result.Ok(parserSelector[format]);
        }

        private void PaintSource(string sourcePath, IParser parser)
        {
            var result = GenerateImage(sourcePath, parser);
            if (result.IsSuccess)
            {
                Console.WriteLine("The path for image");
                Console.WriteLine(result.Value);
            }            
            else
                Console.WriteLine(result.Error);

            Console.WriteLine();
        }

        private Result<string> GenerateImage(string path, IParser parser)
        {
            return parser.Parse(path)
                .Then(preprocessorsApplier.ApplyPreprocessors)
                .Then(filtersApplier.ApplyFilters)
                .Then(container.GetInstance<ITagCreator>().CreateTags)
                .Then(tagPainter.Paint)
                .Then(cloudPainter.Paint);
        }
    }
}
