namespace TagsCloud.Interfaces
{
    public interface IClientValidator
    {
        public char[] ValidChars { get; }

        public ResultHandler<string> ValidateRussianInput(string line);

        public ResultHandler<string> ValidateWrongSymbolsInFileName(string line);

        public ResultHandler<string> ValidateRightTextExtension(string line);

        public ResultHandler<string> ValidateRightPictureExtension(string line);
    }
}