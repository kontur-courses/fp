using Autofac;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TagsCloud;
using TagsCloud.DI;
using TagsCloud.Layouters;
using TagsCloud.Renderers;
using TagsCloud.WordsFiltering;

namespace TagsCloud_console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagsCloudModule());
            var container = builder.Build();

            Parser.Default.ParseArguments<InputOptions>(args).WithParsed(opts =>
            {
                ITagsCloudLayouter selectedLayouter = default;
                ITagsCloudRenderer selectedRenderer = default;
                IFilter[] selectedFilters = default;

                if (DetermineLayouter(opts, container)
                .Then(layouter => selectedLayouter = layouter)
                .Then(layouter => DetermineRenderer(opts, container))
                .Then(renderer => selectedRenderer = renderer)
                .Then(renderer => DetermineFilters(opts, container))
                .Then(filters =>
                {
                    selectedFilters = filters;
                    var wordsLoader = container.Resolve<WordsLoader>();
                    return wordsLoader.LoadWords(opts.InputFile);
                })
                .Then(words =>
                {
                    var wordsFilterer = container.Resolve<WordsFilterer>(new NamedParameter("filters", selectedFilters));
                    return wordsFilterer.FilterWords(words);
                })
                .Then(filteredWords =>
                {
                    var tagCloud = container.Resolve<TagsCloudGenerator>(
                        new NamedParameter("layouter", selectedLayouter),
                        new NamedParameter("renderer", selectedRenderer));
                    return tagCloud.GenerateCloud(filteredWords);
                })
                .Then(image =>
                {
                    var imageSaveHelper = container.Resolve<ImageSaveHelper>();
                    return imageSaveHelper.SaveTo(image, opts.OutputFile);
                })
                .OnFail(msg => Console.Error.WriteLine(msg))
                .IsSuccess)
                    Console.WriteLine("OK");
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static Result<IFilter[]> DetermineFilters(InputOptions opts, IContainer container)
        {
            var knownFilters = container.Resolve<IFilter[]>();
            var parseRes = ParseObjects(knownFilters, opts.Filters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            return parseRes.IsSuccess
                ? parseRes.GetValueOrThrow().Cast<IFilter>().ToArray().AsResult()
                : Result.Fail<IFilter[]>(parseRes.Error);
        }

        private static Result<ITagsCloudLayouter> DetermineLayouter(InputOptions opts, IContainer container)
        {
            var knownLayouters = container.Resolve<ITagsCloudLayouter[]>();
            var parseRes = ParseObjects(knownLayouters, opts.Layouter);
            if (parseRes.IsSuccess)
                return parseRes.GetValueOrThrow().Length != 1
                    ? Result.Fail<ITagsCloudLayouter>($"One layouter must be selected")
                    : (parseRes.GetValueOrThrow()[0] as ITagsCloudLayouter).AsResult();
            else
                return Result.Fail<ITagsCloudLayouter>(parseRes.Error);
        }

        private static Result<ITagsCloudRenderer> DetermineRenderer(InputOptions opts, IContainer container)
        {
            var knownRenderers = container.Resolve<ITagsCloudRenderer[]>();
            var parseRes = ParseObjects(knownRenderers, opts.Renderer);
            if (parseRes.IsSuccess)
                return parseRes.GetValueOrThrow().Length != 1
                    ? Result.Fail<ITagsCloudRenderer>($"One renderer must be selected")
                    : (parseRes.GetValueOrThrow()[0] as ITagsCloudRenderer).AsResult();
            else
                return Result.Fail<ITagsCloudRenderer>(parseRes.Error);
        }

        private static Result<object[]> ParseObjects(IEnumerable<object> knownObjects, params string[] settings)
        {
            var res = new List<object>();

            var regexObjectWithSettings = new Regex(@"(\S+)\((\S+)\)");
            foreach (var item in settings)
            {
                var matchObjectWithSettings = regexObjectWithSettings.Match(item);
                string neededObjectTypeName = matchObjectWithSettings.Success
                    ? matchObjectWithSettings.Groups[1].Value
                    : item;
                var findedObject = knownObjects.FirstOrDefault(o => o.GetType().Name == neededObjectTypeName);
                if (findedObject == null)
                    return Result.Fail<object[]>($"Can't parse object '{item}'");

                if (matchObjectWithSettings.Success)
                {
                    var objectSettings = matchObjectWithSettings.Groups[2].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var parseSettingsResult = ParseSettings(findedObject, objectSettings);
                    if (!parseSettingsResult.IsSuccess)
                        return Result.Fail<object[]>(parseSettingsResult.Error);
                }

                res.Add(findedObject);
            }

            return Result.Ok(res.ToArray());
        }

        private static Result<None> ParseSettings(object obj, string[] options)
        {
            var props = new Dictionary<string, PropertyInfo>();
            foreach (var p in obj.GetType().GetProperties().Where(p => p.CanWrite))
                props.Add(p.Name, p);

            foreach (var option in options)
            {
                var kv = option.Split(':');
                if (kv.Length != 2)
                    return Result.Fail<None>($"Can't parse '{option}' as option of {obj.GetType().Name}");

                var propName = kv[0];
                if (!props.TryGetValue(propName, out var propertyInfo))
                    return Result.Fail<None>($"Can't parse '{option}' as option of {obj.GetType().Name}");

                var parseRes = ParseProperty(kv[1], propertyInfo, obj);
                if (!parseRes.IsSuccess)
                    return parseRes;
            }

            return Result.Ok();
        }

        private static Result<None> ParseProperty(string optionString, PropertyInfo propertyInfo, object obj)
        {
            return Result.OfAction(() =>
            {
                if (propertyInfo.PropertyType == typeof(bool))
                {
                    var val = bool.Parse(optionString);
                    propertyInfo.SetValue(obj, val);
                    return;
                }

                if (propertyInfo.PropertyType == typeof(int))
                {
                    var val = int.Parse(optionString);
                    propertyInfo.SetValue(obj, val);
                    return;
                }

                if (propertyInfo.PropertyType == typeof(System.Drawing.Font))
                {
                    var fontNames = System.Drawing.FontFamily.Families.Select(f => f.Name);
                    var font = new System.Drawing.Font(optionString, 16);
                    propertyInfo.SetValue(obj, font);
                    return;
                }

                if (propertyInfo.PropertyType == typeof(System.Drawing.Color))
                {
                    var knownColorsNames = Enum.GetNames(typeof(System.Drawing.KnownColor));
                    if (!knownColorsNames.Contains(optionString))
                        throw new ArgumentException($"Unknown color name '{optionString}'.");
                    var color = System.Drawing.Color.FromName(optionString);
                    propertyInfo.SetValue(obj, color);
                    return;
                }

                throw new ArgumentException($"Can't parse property of type '{propertyInfo.PropertyType.Name}'");
            }).RefineError($"Can't parse '{optionString}' as value of {propertyInfo.Name}");
        }
    }
}
