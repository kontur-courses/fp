using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class ConsoleClientFunc : IClient
    {
        private static HashSet<string> fontFamilies = FontFamily.Families
           .Select(font => font.Name).ToHashSet();

        public Result<Size> GetImageSize()
        {
            Console.WriteLine("задайте размер изображения(его ширина и высота в пикселях через пробел)");
            var input = Console.ReadLine();
            var digits = input.Split(" ");

            if (digits.Length != 2)
            {
                return Result.Fail<Size>("Размер должен задаваться через пробел и содержать 2 целых числа");
            }
            if (int.TryParse(digits[1], out var resHeight)
                && int.TryParse(digits[0], out var resWidth) 
                && resHeight > 0 && resWidth > 0)
                return new Size(resWidth, resHeight);

            return Result.Fail<Size>("Размер должен задаваться целыми числами");
        }

        private Result<Color> GetColor(string obj)
        {
            Console.WriteLine($"задайте цвет для {obj} в формате RGB(каждый из цветов через пробел)");
            var input = Console.ReadLine();
            var digits = input.Split(" ");

            if (digits.Length != 3)
            {
                return Result.Fail<Color>("Цвет должен задаваться через пробел и содержать 3 целых числа");
            }
            if (byte.TryParse(digits[1], out var green)
                && byte.TryParse(digits[0], out var red)
                && byte.TryParse(digits[2], out var blue))
                return Color.FromArgb(red, green, blue);

            return Result.Fail<Color>("Цвет должен быть в рамках от 0 до 255");
        }

        public Result<FontFamily> GetFontFamily()
        {
            Console.WriteLine("Напишите название шрифта для текста(Arial, Times New Roman, ...)");
            var input = Console.ReadLine();

            if (fontFamilies.Contains(input))
                return new FontFamily(input);

            return Result.Fail<FontFamily>("Шрифт не найден");
        }

        public Result<Color> GetTextColor()
        {
            return GetColor("текста");
        }

        public Result<Color> GetBackgoundColor()
        {
            return GetColor("фона");
        }

        public void ShowPathToNewFile(string path)
        {
            Console.WriteLine($"Изображение сохранено {path}");
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public Result<string> GetNameForImage()
        {
            Console.WriteLine("Придумайте название для изображения");
            return Console.ReadLine();
        }

        public bool IsFinish()
        {
            if (Console.KeyAvailable)
            {
                var cl = Console.ReadKey(true);

                return cl.Modifiers.HasFlag(ConsoleModifiers.Control)
                    && cl.Key.ToString() == "c";
            }
            return false;
        }
    }
}
