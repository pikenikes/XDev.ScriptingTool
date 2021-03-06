﻿namespace XDev.ScriptingTool.ConsolePresentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DependencyInjection;
    using Models;
    using Services;

    internal class Program
    {
        /// <summary>
        /// Executes the program with the specified path parameter.
        /// </summary>
        /// <param name="pathParam">The path parameter.</param>
        private static void Execute(string pathParam)
        {
            string specifiedPath;
            if (pathParam == null)
            {
                Console.WriteLine(Consts.AssumingCurrentPathMessage);
                specifiedPath = ".";
            }
            else
            {
                string[] pathParamSplit = pathParam.Split(Consts.ParamValueSeparator);

                if (pathParam == null || pathParamSplit.Length != 2)
                {
                    Console.WriteLine(Consts.UsageMessage);
                    return;
                }

                if (!string.Equals(pathParamSplit[0], Consts.PathParamName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(Consts.UsageMessage);
                    return;
                }

                specifiedPath = pathParamSplit[1];
            }

            ConsoleBootrapper consoleBootstrapper = new ConsoleBootrapper();
            consoleBootstrapper.Bootstrap();

            IDependencyInjectionContainer dependencyInjectionContainer = consoleBootstrapper.DependencyInjectionContainer;

            PathResolver pathResolver = new PathResolver();
            string resolvedPath = pathResolver.ResolvePath(specifiedPath);

            IFileDiscoveryService filePathDiscoveryService = dependencyInjectionContainer.Resolve<IFileDiscoveryService>();
            IEnumerable<FileDiscoveryInfo> scriptFiles = filePathDiscoveryService.DiscoverFiles(resolvedPath);

            IScriptingService scriptingService = dependencyInjectionContainer.Resolve<IScriptingService>();
            foreach (var scriptFile in scriptFiles)
            {
                ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();

                while (!Program.ValidKey(consoleKeyInfo))
                {
                    Console.Write($"Run script '{ scriptFile.FileName }'? (y/n) ");
                    consoleKeyInfo = Console.ReadKey();
                    Console.WriteLine();
                }

                if (consoleKeyInfo.Key == ConsoleKey.Y)
                {
                    scriptingService.ExecuteScriptAsync(scriptFile).Wait();
                }
            }
        }

        /// <summary>
        /// Main (entry point).
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine(Consts.StartMessage);
            string pathToDiscover = args.FirstOrDefault();
            Program.Execute(pathToDiscover);
            Console.WriteLine(Consts.ExitMessage);
            Console.ReadLine();
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <param name="consoleKeyInfo">The console key information.</param>
        /// <returns></returns>
        private static bool ValidKey(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                case ConsoleKey.N:
                    return true;

                default:
                    return false;
            }
        }
    }
}