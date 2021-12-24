using System.Collections.Generic;

namespace TagCloud.Templates;

public interface ITemplateCreator
{
    public Result<ITemplate> GetTemplate(IEnumerable<string> words);
}