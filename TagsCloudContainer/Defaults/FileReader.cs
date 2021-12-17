using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.SettingsProviders;

namespace TagsCloudContainer.Defaults;

public class FileReader : ITextReader
{
    private readonly List<FileInfo> paths = new();

    public FileReader(InputSettings settings) : this(settings.Paths)
    {

    }

    protected FileReader(string[] paths)
    {
        foreach (var path in paths)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
                throw new FileNotFoundException($"Could not find file with name '{path}'");
            this.paths.Add(file);
        }
    }

    public IEnumerable<string> ReadLines()
    {
        foreach (var file in paths)
        {
            using var fileStream = file.OpenText();
            var line = fileStream.ReadLine();
            while (!fileStream.EndOfStream)
            {
                yield return line!;
                line = fileStream.ReadLine();
            }

            yield return line!;
        }
    }
}
