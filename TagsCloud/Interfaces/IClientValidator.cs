namespace TagsCloud.Interfaces
{
    public interface IClientValidator
    {
        public char[] ValidChars { get; }

        public Result<string> ValidateRussianInput(string line);

        public Result<string> ValidateWrongSymbolsInPath(string line);

        public Result<string> ValidateRightTextExtension(string line);

        public Result<string> ValidateRightPictureExtension(string line);
    }
}