using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
    public interface IClientValidator
    {
        public HashSet<char> ValidChars { get; }

        public ResultHandler<string> ValidateRussianInput(string line);

        public ResultHandler<string> ValidateWrongSymbolsInFileName(string line);

        public ResultHandler<string> ValidateRightTextExtension(string line);

        public ResultHandler<string> ValidateRightPictureExtension(string line);
    }
}