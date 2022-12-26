namespace TagCloudContainer.Core.Interfaces;

public interface ILinesValidator
{
    public IEnumerable<string> Validate(IEnumerable<string> lines);
}