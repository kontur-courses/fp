using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI.Menu
{
    public class MainMenu : IMainMenu
    {
        private readonly Dictionary<int, Category> categories;
        public Category[] Categories => categories.Values.ToArray();
        
        private readonly IResultHandler handler;

        public MainMenu(Dictionary<int, Category> categories, IResultHandler handler)
        {
            this.handler = handler;
            this.categories = categories;
        }

        private void GetCategories()
        {
            handler.AddHandledText("To choose category write position key, for example '1'");
            handler.AddHandledText("To exit write 'Q'");
            foreach (var pos in categories.Keys)
            {
                handler.AddHandledText($"{pos}. {categories[pos].Name}");
            }
        }

        public void ChooseCategory()
        {
            GetCategories();
            while (true) 
                handler.Handle(ChooseCategoryAction);
        }

        private void ChooseCategoryAction()
        {
            var keyStr = handler.GetText();
            if (keyStr == "Q")
                Environment.Exit(0);
            if (int.TryParse(keyStr, out var key))
                if (categories.ContainsKey(key))
                {
                    categories[key].ChooseMenuItem();
                    GetCategories();
                }
                else
                    throw new Exception("No Category with that key");
            else
                throw new Exception("Key should be int!");
        }
    }
}