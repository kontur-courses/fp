using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud
{
    public static class SpeechPartsForm
    {
        public static SpeechPartsForm<T> For<T>(T[] items) where T : SpeechPart
        {
            return new SpeechPartsForm<T>(items);
        }
    }

    public class SpeechPartsForm<T> : CheckedListForm<T>
    {
        public SpeechPartsForm(T[] items) : base(items)
        {
        }

        protected override void AddItems(T[] items)
        {
            foreach (var item in items)
                checkedListBox.Items.Add(Enum.GetName(typeof(SpeechPartEnum), (item as SpeechPart).Value));
        }
    }
}
