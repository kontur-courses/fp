using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.WordsFileReaders;

public class TxtFileReader : StandardFileReader
{
    public TxtFileReader(IWordsPathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
    }

    public override string SupportedExtension => ".txt";

    protected override string InternalReadFile(string path) =>
        File.ReadAllText(path);
}