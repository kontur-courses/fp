﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using App.Infrastructure.FileInteractions.Writers;
using App.Infrastructure.Words.Preprocessors;

namespace App.Implementation.Words.Preprocessors
{
    public class ToInitFormPreprocessor : IPreprocessor
    {
        private readonly string inputFilePath;
        private readonly string pathToInitFormExe;
        private readonly ILineWriter writer;

        public ToInitFormPreprocessor(ILineWriter writer)
        {
            this.writer = writer;
            inputFilePath = ".stem_input";
            pathToInitFormExe = "mystem.exe";
        }

        public Result<IEnumerable<string>> Preprocess(IEnumerable<string> words)
        {
            PrepareWordsForOuterLibrary(words);

            return TryToLeadWordsToInitForm();
        }

        private Result<IEnumerable<string>> TryToLeadWordsToInitForm()
        {
            var initialLeadingFormProcess = CreateInitialLeadingFormProcess(inputFilePath);
            using (initialLeadingFormProcess)
            {
                try
                {
                    var wordsInitForms = new List<string>();
                    initialLeadingFormProcess.Start();

                    while (!initialLeadingFormProcess.StandardOutput.EndOfStream)
                        wordsInitForms.Add(initialLeadingFormProcess.StandardOutput.ReadLine());

                    return new Result<IEnumerable<string>>(null, wordsInitForms);
                }
                catch (Exception e)
                {
                    var msg = $"Error during {pathToInitFormExe} work. {e.Message}";
                    return Result.Fail<IEnumerable<string>>(msg);
                }
            }
        }

        private void PrepareWordsForOuterLibrary(IEnumerable<string> words)
        {
            var cwd = Directory.GetCurrentDirectory();
            var streamWriter = new StreamWriter(inputFilePath, false, Encoding.UTF8);

            writer.WriteLinesTo(streamWriter, words);
        }

        private Process CreateInitialLeadingFormProcess(string pathToFile)
        {
            return new Process
            {
                StartInfo = BuildStartInfo(pathToFile)
            };
        }

        private ProcessStartInfo BuildStartInfo(string pathToFile)
        {
            return new ProcessStartInfo
            {
                FileName = pathToInitFormExe,
                Arguments = $"-nld {pathToFile}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            };
        }
    }
}