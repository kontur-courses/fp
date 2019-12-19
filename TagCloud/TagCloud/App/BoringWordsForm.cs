namespace TagCloud
{
    public static class BoringWordsForm
    {
        public static BoringWordsForm<T> For<T>(T[] items) where T : BoringWord
        {
            return new BoringWordsForm<T>(items);
        }
    }

    public class BoringWordsForm<T> : CheckedListForm<T>
    {
        public BoringWordsForm(T[] items) : base(items)
        {
        }

        protected override void AddItems(T[] items)
        {
            foreach (var item in items)
                checkedListBox.Items.Add((item as BoringWord).Value);
        }
    }
}
