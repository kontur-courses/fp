namespace TagsCloud.Interfaces
{
	public interface ILayoutPainter
	{
		Result<None> PaintTags(Layout layout);
	}
}