using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI.Menu
{
    public class Category : ICategory
    {
        private readonly Dictionary<int, MenuItem> items;
        private readonly IResultHandler handler;
        public MenuItem[] Items => items.Values.ToArray();
        public string Name { get; }

        public Category(Dictionary<int, MenuItem> items, string name, IResultHandler handler)
        {
            this.items = items;
            this.handler = handler;
            Name = name;
        }

        private void GetMenuItems()
        {
            handler.AddHandledText($"To choose Menu action write position key, for example '1'");
            handler.AddHandledText($"To return to main menu write 'Q'");
            foreach (var pos in items.Keys)
            {
                handler.AddHandledText($"{pos}. {items[pos].Name}");
            }
        }

        public void ChooseMenuItem()
        {
            GetMenuItems();
            handler.Handle(ChooseMenuItemAction);
        }

        private void ChooseMenuItemAction()
        {
            var keyStr = handler.GetText();
            if (keyStr == "Q")
                return;
            if (int.TryParse(keyStr, out var key))
                if (items.ContainsKey(key))
                {
                    items[key].Perform();
                    GetMenuItems();
                }
                else
                    throw new Exception("No menu item with that key");
            else
                throw new Exception("Key should be int!");
        }
    }
}