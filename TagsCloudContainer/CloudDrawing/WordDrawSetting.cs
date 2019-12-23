using System;
using System.Drawing;
using ResultOf;

namespace CloudDrawing
{
    public class WordDrawSettings
    {
        public WordDrawSettings(string familyName, Brush brush, StringFormat stringFormat, bool haveDelineation)
        {
            FamilyName = familyName;
            Brush = brush;
            StringFormat = stringFormat;
            HaveDelineation = haveDelineation;
        }

        public string FamilyName { get; }
        public Brush Brush { get; }
        public StringFormat StringFormat { get; }
        public bool HaveDelineation { get; }

        public static Result<WordDrawSettings> GetWordDrawSettings(string famyilyNameFont, string colorBrush,
            bool haveDelineation)
        {
            return Result.Validate(colorBrush, color => Enum.TryParse(color, true, out KnownColor knownColor),
                    $"Не существует такого цвета {colorBrush} заданного для кисти")
                .Then(Color.FromName)
                .Then(color => new SolidBrush(color))
                .Then(brush => new WordDrawSettings(famyilyNameFont,
                    brush,
                    new StringFormat
                    {
                        LineAlignment = StringAlignment.Center
                    },
                    haveDelineation));
        }
    }
}