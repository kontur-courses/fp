namespace TagCloud.selectors
{
    public class BoringChecker : IChecker<string>
    {
        public Result<string> IsValid(string source) 
            => source.Length > 3 ? Result.Ok(source) : Result.Fail<string>(ResultErrorType.IncorrectWordError);
    }
}