using System.Collections.Generic;

namespace TagsCloudContainer.TokensGenerator
{
    public interface ITokensParser
    {
        Result<IEnumerable<string>> GetTokens(string str);
    }
}