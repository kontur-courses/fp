using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImageSizeSettingsManager : IMultiInputModifierManager
    {
        private readonly Func<IImageSettingsProvider> imageSettingsProvider;
        private readonly Regex regex;

        private readonly Dictionary<string, Dictionary<string, Func<Result<string>>>> modifiers;

        public ImageSizeSettingsManager(Func<IImageSettingsProvider> imageSettingsProvider)
        {
            this.imageSettingsProvider = imageSettingsProvider;
            regex = new Regex(@"^(?<width>\d+)\s+(?<height>\d+)$");
            modifiers = new Dictionary<string, Dictionary<string, Func<Result<string>>>>()
            {
                {
                    "Width", new Dictionary<string, Func<Result<string>>>()
                    {
                        {
                            "-10", () =>
                            {
                                imageSettingsProvider().Width -= 10;
                                return "";
                            }
                        },
                        {
                            "-1", () =>
                            {
                                imageSettingsProvider().Width--;
                                return "";
                            }
                        },
                        {
                            "+1", () =>
                            {
                                imageSettingsProvider().Width++;
                                return "";
                            }
                        },
                        {
                            "+10", () =>
                            {
                                imageSettingsProvider().Width += 10;
                                return "";
                            }
                        },
                    }
                },
                {
                    "Height", new Dictionary<string, Func<Result<string>>>()
                    {
                        {
                            "-10", () =>
                            {
                                imageSettingsProvider().Height -= 10;
                                return "";
                            }
                        },
                        {
                            "-1", () =>
                            {
                                imageSettingsProvider().Height--;
                                return "";
                            }
                        },
                        {
                            "+1", () =>
                            {
                                imageSettingsProvider().Height++;
                                return "";
                            }
                        },
                        {
                            "+10", () =>
                            {
                                imageSettingsProvider().Height += 10;
                                return "";
                            }
                        },
                    }
                },
            };
        }

        public string Title => "Size";
        public string Help => "Input image width and height separated by space";

        public Result<string> TrySet(string input)
        {
            var match = regex.Match(input);
            if (!match.Success)
                return Result.Fail<string>("Incorrect input format ([width], [height])");
            imageSettingsProvider().Width = int.Parse(match.Groups["width"].Value);
            imageSettingsProvider().Height = int.Parse(match.Groups["height"].Value);
            return Get();
        }

        public string Get()
        {
            var settings = imageSettingsProvider();
            return $"{settings.Width} {settings.Height}";
        }

        public Dictionary<string, IEnumerable<string>> GetModifiers()
        {
            return modifiers.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.Select(pair => pair.Key)
            );
        }

        public Result<string> ApplyModifier(string type, string modifier)
        {
            return Result.Of(() =>
            {
                modifiers[type][modifier]();
                return "";
            }).ReplaceError(s => "Cannot apply modifier");
        }
    }
}