using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Spirals;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainer.ConsoleInterface.ConsoleActions
{
    public class CloudSettingsAction : IUIAction
    {
        private readonly CloudSettings cloudSettings;
        private readonly Dictionary<string, ITagPainter> painterSelector;
        private readonly Dictionary<string, ISpiral> spiralSelector;

        public CloudSettingsAction(CloudSettings cloudSettings,
            ITagPainter[] painters, ISpiral[] spirals)
        {
            this.cloudSettings = cloudSettings;

            painterSelector = new Dictionary<string, ITagPainter>();
            for (var i = 1; i <= painters.Length; i++)
                painterSelector.Add(i.ToString(), painters[i - 1]);

            spiralSelector = new Dictionary<string, ISpiral>();
            for (var i = 1; i <= spirals.Length; i++)
                spiralSelector.Add(i.ToString(), spirals[i - 1]);
        }

        public string GetDescription() => "Cloud settings";

        public void Handle()
        {
            Console.WriteLine("1 - Tag painting");
            Console.WriteLine("2 - Cloud view");
            Console.WriteLine("3 - Back");
            var answer = Console.ReadLine();
            Console.WriteLine();

            switch (answer)
            {
                case "1":
                    TagPaintingKey();
                    break;
                case "2":
                    CloudViewKey();
                    break;
                default:
                    return;
            }
        }

        private void TagPaintingKey()
        {
            foreach (var (key, painter) in painterSelector.OrderBy(pair => pair.Key))
                Console.WriteLine($"{key} - {painter.Name}");
            Console.WriteLine($"{painterSelector.Count + 1} - Exit");
            var answer = Console.ReadLine() ?? "";
            Console.WriteLine();
            if (!painterSelector.ContainsKey(answer))
                return;
            cloudSettings.Painter = painterSelector[answer];
        }

        private void CloudViewKey()
        {
            foreach (var (key, spiral) in spiralSelector.OrderBy(pair => pair.Key))
                Console.WriteLine($"{key} - {spiral.Name}");
            Console.WriteLine($"{spiralSelector.Count + 1} - Exit");
            var answer = Console.ReadLine() ?? "";
            Console.WriteLine();
            if (!spiralSelector.ContainsKey(answer))
                return;
            cloudSettings.Spiral = spiralSelector[answer];
        }
    }
}
