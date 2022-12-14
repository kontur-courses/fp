using TagCloud.Infrastructure;
using TagCloud.Parser.ParsingConfig;

namespace TagCloud.Parser;

public class PlainTextParser : ITagParser
{
    private IParsingConfig parsingConfig;

    public PlainTextParser(IParsingConfig parsingConfig)
    {
        this.parsingConfig = parsingConfig;
    }

    public Result<TagMap> Parse(string filepath)
    {
        if (!File.Exists(filepath))
            return new Result<TagMap>($"Could not find file {filepath}.");
        
        var tagMap = new TagMap();
        
        foreach (var line in File.ReadLines(filepath))
        {
            var word = line.ToLower();
            if (word.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 1) 
                return new Result<TagMap>($"Could not parse file {filepath}: wrong text format.");
            
            if (!parsingConfig.IsWordExcluded(word))
                tagMap.AddWord(word);
        }

        return tagMap;
    }
}