namespace TagCloud.Infrastructure.Pipeline.Common;

public interface IOutputPathProvider
{
    string OutputPath { get; }
    public string OutputFormat { get; }
}