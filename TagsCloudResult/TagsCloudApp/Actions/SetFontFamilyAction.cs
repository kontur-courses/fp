﻿using System.Drawing;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetFontFamilyAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IFontFamilySettings settings;

        public SetFontFamilyAction(IRenderArgs renderArgs, IFontFamilySettings settings)
        {
            this.renderArgs = renderArgs;
            this.settings = settings;
        }

        public Result<None> Perform()
        {
            return Result.Of(() => new FontFamily(renderArgs.FontFamily))
                .Then(fontFamily =>
                {
                    settings.FontFamily = fontFamily;
                })
                .ReplaceError(_ => $"Can't get FontFamily from: {renderArgs.FontFamily}.");
        }
    }
}