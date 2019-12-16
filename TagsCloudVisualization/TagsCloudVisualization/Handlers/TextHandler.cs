using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spire.Doc;
using TagsCloudVisualization.Results;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.Handlers
{
    public class TextHandler
    {
        private readonly Dictionary<string, Func<string, string>> fileOpener 
            = new Dictionary<string, Func<string, string>>
        {
            {".txt", name =>
                {
                    using (var file = new StreamReader(name))
                        return file.ReadToEnd().Replace(Environment.NewLine, " ");
                }
            },
            {".doc", name =>
                {
                    var doc = new Document(name, FileFormat.Doc);
                    return doc.GetText().Replace(Environment.NewLine, " ");
                }
            },
            {".docx", name =>
                {
                    var doc = new Document(name, FileFormat.Docx);
                    return doc.GetText().Replace(Environment.NewLine, " ");
                }
            },
        };

        private readonly TextSettings textSettings;

        public TextHandler(TextSettings textSettings)
        {
            this.textSettings = textSettings;
        }

        public Dictionary<string, int> GetFrequencyDictionary()
        {
            var frequencyDict = new Dictionary<string, int>();
            var result = GetText(textSettings.FileExtention, textSettings.PathToFile);
            if (!result.IsSuccess)
                Environment.Exit(1);
            foreach (var value in textSettings.Filter.GetFilteredValues(result.Value))
                    frequencyDict[value] = frequencyDict.TryGetValue(value, out var frequency) ? ++frequency : 1;
            return frequencyDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
        }

        private Result<string> GetText(string extention, string fileName)
            => Result.Ok(fileName)
                     .Then(name => fileOpener[extention](name))
                     .OnFail(error => Console.WriteLine($"Please, check path to file, we can't find {fileName}. The error was: {error}"));
    }
}
    