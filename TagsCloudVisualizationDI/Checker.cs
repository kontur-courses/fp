using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace TagsCloudVisualizationDI
{
    public static class Checker
    {
        public static void CheckPathToFile(string pathToFile)
        {
            Result.OnFalse(File.Exists(pathToFile),$"pathToFile {pathToFile}  is not exist", (error) => PrintAboutFail(error));
        }


        public static void CheckPathToDirectory(string pathToDirectory)
        {
            Result.OnFalse(Directory.Exists(pathToDirectory), $"pathToDirectory {pathToDirectory}  is not exist", (error) => PrintAboutFail(error));
        }

        /*
        public static Result<None> CheckPathToDirectory(string pathToSave)
        {
            Console.WriteLine(pathToSave);
            var a = Result.OfAction(() => Directory.Exists(pathToSave));
            var b = a.Error;
            Console.WriteLine("@@@");
            Console.WriteLine(Directory.Exists(pathToSave));
            Console.WriteLine(a.Value);
            Console.WriteLine(a.Error);
            Console.WriteLine(a.IsSuccess);


            //Console.WriteLine(Result.OfAction(() => Directory.Exists(pathToSave)).Value);
            return Result.OfAction(() => Directory.Exists(pathToSave));
            //.OnFail(exc => PrintAboutFail(exc));
        }
        */
        private static void PrintAboutFail(string message)
        {
            //throw error.GetBaseException();
            throw new Exception(message);
        }
    }
}
