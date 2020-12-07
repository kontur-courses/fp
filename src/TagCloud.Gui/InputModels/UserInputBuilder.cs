using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Gui.Localization;

namespace TagCloud.Gui.InputModels
{
    public class UserInputBuilder : IUserInputBuilder
    {
        private readonly ILocalizationProvider provider;

        public UserInputBuilder(ILocalizationProvider provider)
        {
            this.provider = provider;
        }

        public UserInputField Field(UiLabel key) => new UserInputField(provider.GetLabel(key));

        public UserInputSizeField Size(UiLabel key, bool showAsPoint = false) =>
            new UserInputSizeField(provider.GetLabel(key),
                provider.GetLabel(showAsPoint ? UiLabel.XPoint : UiLabel.Width),
                provider.GetLabel(showAsPoint ? UiLabel.YPoint : UiLabel.Height));

        public UserInputMultipleOptionsChoice<TService> MultipleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key) =>
            new UserInputMultipleOptionsChoice<TService>(provider.GetLabel(key), SelectorItems(source));

        public UserInputOneOptionChoice<TService> SingleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key) =>
            new UserInputOneOptionChoice<TService>(provider.GetLabel(key), SelectorItems(source));

        public UserInputColor Color(Color defaultValue, UiLabel key) =>
            new UserInputColor(provider.GetLabel(key), defaultValue);

        public UserInputColorPalette ColorPalette(UiLabel key, params Color[] initialValue) =>
            new UserInputColorPalette(provider.GetLabel(key), initialValue,
                provider.GetLabel(UiLabel.ButtonAdd),
                provider.GetLabel(UiLabel.ButtonRemove));

        public UserInputOneOptionChoice<TService> ServiceChoice<TService>(IEnumerable<TService> source, UiLabel key) =>
            SingleChoice(source.Where(x => x != null).ToDictionary(x => provider.Get(x.GetType())), key);

        public UserInputOneOptionChoice<TEnum> EnumChoice<TEnum>(UiLabel key) where TEnum : struct, Enum =>
            SingleChoice(Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToDictionary(provider.Get), key);

        private UserInputSelectorItem<T>[] SelectorItems<T>(IDictionary<string, T> source) =>
            source.Select(x => new UserInputSelectorItem<T>(x.Key, x.Value))
                .OrderBy(x => x.Name)
                .ToArray();
    }
}