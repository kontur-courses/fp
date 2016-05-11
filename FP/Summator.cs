using System;
using System.IO;
using System.Linq;

namespace FP
{
	public class Summator
	{
		private readonly Func<DataSource> openDatasource;
		private readonly ISumFormatter formatter;
		private readonly string outputFilename;
		/*
		Отрефакторите код.
			1. Отделите максимум логики от побочных эффектов.
			2. Создайте нужные вам методы.
			3. Сделайте так, чтобы максимум кода оказалось внутри универсальных методов, потенциально полезных в других местах программы.
		*/

		public Summator(Func<DataSource> openDatasource, ISumFormatter formatter, string outputFilename)
		{
			this.openDatasource = openDatasource;
			this.formatter = formatter;
			this.outputFilename = outputFilename;
		}

		public void Process()
		{
			using(var input = openDatasource())
			using (var writer = new StreamWriter(outputFilename))
			{
				var c = 0;
				while (true)
				{
					string[] record = input.NextRecord();
					if (record == null) break;
					c++;
					var nums = record.Select(part => Convert.ToInt32(part, 16)).ToArray();
					var sum = nums.Sum();
					var text = formatter.Format(nums, sum);
					writer.WriteLine(text);
					if (c % 100 == 0)
						Console.WriteLine("processed {0} items", c);
				}
			}
		}
	}
}