using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloud.Infrastructure;
using TagCloud.Parser.ParsingConfig;

namespace TagCloud.Parser;

public class WordDocumentParser : ITagParser
{
    private IParsingConfig parsingConfig;

    public WordDocumentParser(IParsingConfig parsingConfig)
    {
        this.parsingConfig = parsingConfig;
    }
    
    public Result<TagMap> Parse(string filepath)
    {
        if (!File.Exists(filepath))
            return new Result<TagMap>($"Could not find file {filepath}.");
        
        using var document = WordprocessingDocument.Open(filepath, false);
        if (document.MainDocumentPart?.RootElement == null)
            return new Result<TagMap>($"Could not parse file {filepath}: document lacks the main part.");
        
        var paragraphs = document.MainDocumentPart.RootElement.Descendants<Paragraph>();
        var tagMap = new TagMap();
        foreach (var paragraph in paragraphs)
        {
            var word = paragraph.InnerText.ToLower();
            if (word.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 1) 
                return new Result<TagMap>($"Could not parse file {filepath}: wrong text format.");

            if (!parsingConfig.IsWordExcluded(word))
                tagMap.AddWord(word);
        }

        return tagMap;
    }
}