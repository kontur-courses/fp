namespace TagCloudGenerator.ResultPattern
{
    public interface IResult
    {
        string Error { get; }
        bool IsSuccess { get; }
    }
}