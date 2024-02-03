using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Domain.Visualizer.Interfaces;
using TagCloud.Utils.Files.Interfaces;
using TagCloud.Utils.Images.Interfaces;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class DrawCommand : ICommand
{
    private readonly IVisualizer _visualizer;
    private readonly IImageWorker _imageWorker;
    private readonly FileSettings _fileSettings;
    private readonly IWordsService _wordsService;

    public DrawCommand(
        IVisualizer visualizer, 
        IImageWorker imageWorker, 
        FileSettings fileSettings,
        IWordsService wordsService)
    {
        _visualizer = visualizer;
        _imageWorker = imageWorker;
        _fileSettings = fileSettings;
        _wordsService = wordsService;
    }
    
    public string Trigger => "draw";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result.OfAction(() => 
            {
                using var image = _visualizer.Visualize(_wordsService.GetWords(_fileSettings.FileFromWithPath));
                _imageWorker.SaveImage(
                    image, 
                    _fileSettings.OutPathToFile,
                    _fileSettings.ImageFormat, 
                    _fileSettings.OutFileName);
            
                Console.WriteLine($"Изображение было сохранено по пути {Path.GetFullPath(Path.Combine(_fileSettings.OutPathToFile, _fileSettings.OutFileName))}"); 
            })
            .Then(() => true);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine + 
               "Не имеет параметров";
    }
    
    public string GetShortHelp()
    {
        return Trigger + " позволяет нарисовать облако тегов";
    }
}