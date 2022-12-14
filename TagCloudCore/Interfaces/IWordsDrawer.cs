using System.Drawing;

namespace TagCloudCore.Interfaces;

public interface IWordsDrawer
{
    public Size GetRectSizeFor(string word);
    
}