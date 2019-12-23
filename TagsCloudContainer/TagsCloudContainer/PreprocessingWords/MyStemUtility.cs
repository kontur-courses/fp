using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ResultOf;

namespace TagsCloudContainer.PreprocessingWords
{
    public class MyStemUtility : IPreprocessingWords
    {
        private readonly ICreateProcess createProcess;
        private readonly string flags;
        private readonly string pathMyStemUtility;

        public MyStemUtility(ICreateProcess createProcess)
        {
            this.createProcess = createProcess;
            pathMyStemUtility = Environment.CurrentDirectory + @"\mystem.exe";
            flags = "-nig --format json";
        }

        public Result<IEnumerable<string>> Preprocessing(IEnumerable<string> strings)
        {
            return strings.AsResult().Then(PreprocessingFunc).RefineError("Ошибка работы с MyStemUtility");
        }

        private IEnumerable<string> PreprocessingFunc(IEnumerable<string> strings)
        {
            var pathTempFile = Path.GetTempFileName();
            try
            {
                using (var sw = File.CreateText(pathTempFile))
                {
                    foreach (var str in strings)
                        sw.WriteLine(str);
                }

                return createProcess
                    .GetResult(pathMyStemUtility, flags + " " + pathTempFile)
                    .Select(s => JsonConvert.DeserializeObject<MyStemOutput>(s).GetPrimaryFormOfNouns())
                    .Where(s => s != null);
            }
            finally
            {
                File.Delete(pathTempFile);
            }
        }
    }
}