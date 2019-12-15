using System;
using System.IO;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class TxtReader: ITextReader
	{
		private readonly string fileName;
		private readonly IExceptionHandler exceptionHandler;

		public TxtReader(string fileName, IExceptionHandler exceptionHandler)
		{
			this.fileName = fileName;
			this.exceptionHandler = exceptionHandler;
		}

		public Result<string[]> Read()
		{
			try
			{
				var lines = File.ReadAllLines(fileName);
				return lines;
			}
			catch (Exception e)
			{
				return Result.Fail<string[]>(e);
			}
		}
	}
}