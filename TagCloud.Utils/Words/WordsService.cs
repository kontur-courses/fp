using TagCloud.Utils.Files.Interfaces;
using TagCloud.Utils.ResultPattern;
using IWordsService = TagCloud.Utils.Words.Interfaces.IWordsService;

namespace TagCloud.Utils.Words;

public class WordsService : IWordsService
{
    private readonly IFileService _fileService;
    
    public WordsService(IFileService fileService)
    {
        _fileService = fileService;
    }
    
    public Result<IEnumerable<string>> GetWords(string path)
    {
        return _fileService.CheckExistance(path)
            .Then(() => File.ReadLines(path));
    }
}