using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Processing.Converting;
using TagsCloudContainer.Processing.Filtering;
using TagsCloudContainer.Ui;

namespace TagsCloudContainer.Settings
{
    public static class SettingsCreator
    {
        public static ParserSettings CreateParserSettings(Options options)
        {
            var filters = new List<IWordFilter>();
            var converters = new List<IWordConverter>();

            filters.Add(new DefaultFilter());
            converters.Add(new DefaultConverter());

            if (options.Common)
                filters.Add(new CommonWordsFilter());

            if (options.InitialForm)
                converters.Add(new InitialFormConverter());

            if (!string.IsNullOrEmpty(options.BlackListFile))
            {
                var blackListFilter = BlackListFilter.FromFile(options.BlackListFile);
                if (blackListFilter.IsSuccess)
                    filters.Add(blackListFilter.Value);
            }


            return new ParserSettings(filters.ToArray(), converters.ToArray());
        }

        public static Result<ImageSettings> CreateImageSettings(Options options)
        {
            return File.Exists(options.SettingsFile)
                ? ImageSettings.FromJson(File.ReadAllText(options.SettingsFile))
                : Result.Fail<ImageSettings>("Settings file wasn't found");
        }
    }
}