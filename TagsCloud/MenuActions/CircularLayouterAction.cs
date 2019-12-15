using System;
using System.Threading;
using TagsCloud.Interfaces;

namespace TagsCloud.MenuActions
{
	public class CircularLayouterAction: IMenuAction
	{
		private readonly ILayoutPainter painter;
		private readonly ILayoutConstructor layoutConstructor;
		private readonly SpiralParameters parameters;
		private readonly IExceptionHandler exceptionHandler;
		public string Category { get; } = "Создать облако тегов";
		public string Name { get; } = "Circular layouter";
		public string Description { get; } = "Располагает слова вокруг центра";

		public CircularLayouterAction(ILayoutPainter painter, ILayoutConstructor layoutConstructor,
			SpiralParameters parameters, IExceptionHandler exceptionHandler)
		{
			this.painter = painter;
			this.layoutConstructor = layoutConstructor;
			this.parameters = parameters;
			this.exceptionHandler = exceptionHandler;
		}
		
		public void Perform()
		{
			SettingsForm.For(parameters).ShowDialog();

			ValidateParameters()
				.Then(_ => layoutConstructor.GetLayout())
				.Then(layout => painter.PaintTags(layout))
				.OnFail(exceptionHandler.Handle);
		}
		
		private Result<None> ValidateParameters()
		{
			if (!(Math.Abs(parameters.Density) < double.Epsilon) &&
			    !(Math.Abs(parameters.AngleStepDegrees) < double.Epsilon))
				return Result.Ok();

			return Result.Fail<None>(new ArgumentException("Spiral parameters can't be zero"));
		}
	}
}