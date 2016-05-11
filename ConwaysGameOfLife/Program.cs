using Ninject;
using System;

namespace ConwaysGameOfLife
{
	public static class Program
	{
		private static void Main()
		{
			var container = new StandardKernel();
			container.Bind<IGameUi>().To<ConsoleUi>();
			container.Bind<Size>().ToConstant(new Size(60, 20));
			container.Bind<Game>().ToSelf()
				.OnActivation(g => g.Revive(Patterns.GetGlider(new Point(25, 8))));

			var game = container.Get<Game>();
			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.Key == ConsoleKey.Escape) break;
				game.Step();
			}
		}
	}
}