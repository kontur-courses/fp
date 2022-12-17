﻿using TagCloudApp.Domain;

namespace TagCloudApp.Infrastructure;

public static class UiActionExtensions
{
    public static ToolStripItem[] ToMenuItems(this IEnumerable<IUiAction> actions)
    {
        return actions.GroupBy(a => a.Category)
            .OrderBy(a => a.Key)
            .Select(g => CreateTopLevelMenuItem(g.Key, g.ToList()))
            .Cast<ToolStripItem>()
            .ToArray();
    }

    private static ToolStripMenuItem CreateTopLevelMenuItem(MenuCategory category, IList<IUiAction> items)
    {
        var menuItems = items.Select(a => a.ToMenuItem()).ToArray();
        return new ToolStripMenuItem(category.GetDescription(), null, menuItems);
    }

    public static ToolStripItem ToMenuItem(this IUiAction action)
    {
        return
            new ToolStripMenuItem(action.Name, null, (sender, args) => action.Perform())
            {
                ToolTipText = action.Description,
                Tag = action
            };
    }
}