using System;
using System.Drawing;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Output
{
    public class FileWriter : IWriter
    {
        public Result WriteToFile(byte[] bytes, string path)
        {
            if (File.Exists(path))
                return Result.Fail("Output file already exists");

            var image = Image.FromStream(new MemoryStream(bytes));
            try
            {
                image.Save(path);
            }
            catch (Exception)
            {
                return Result.Fail("Bad output file");
            }

            return Result.Ok();
        }
    }
}