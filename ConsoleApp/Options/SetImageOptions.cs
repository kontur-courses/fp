using CommandLine;
using SixLabors.Fonts;

namespace ConsoleApp.Options;

[Verb("image", HelpText = "Настройка изображения")]
public class SetImageOptions: IOptions
{
    [Option('c', "color", HelpText = "Основной цвет")]
    public string PrimaryColor { get; set; }

    [Option('b', "background", HelpText = "Цвет заднего фона")]
    public string BackgroundColor { get; set; }

    [Option('w', "width", HelpText = "Ширина")] 
    public int Width { get; set; }

    [Option('h', "height", HelpText = "Высота")] 
    public int Height { get; set; }
    
    [Option('f', "font",HelpText = "Шрифт")]
    public Font Font { get; set; }
}