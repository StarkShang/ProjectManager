using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Microsoft.CodeAnalysis;
using ProjectFinder.Command;

namespace ProjectFinder
{
    class Program
    {
        const string SolutionFileSuffix = "sln";
        const string RootName = "root";

        static void Main(string[] args)
        {
            // var (currentWorkDirectory, projectName) = ParseArguments(args);
            // // Get sln file from current/parent directories.
            // // If multiple sln files is found, the nearest one is selected.
            // var slnFileInfo = SolutionManager.GetSolutionFile(currentWorkDirectory);
            // // Get projects which belongs to the sln file.
            // var projects = SlnFileParser.ParseSolutionFile(slnFileInfo);
            // // Get target project path.
            // // Allows partial match.
            // // If multiple projects matched, an exception is throw.
            // var targetPath = GetTargetPath(projectName, projects);
            // Console.Write(targetPath);

            Parser.Default.ParseArguments<MainOptions,AddOptions,DeleteOptions,ListOptions>(args)
                .WithParsed<MainOptions>(opts => opts.Execte())
                .WithParsed<AddOptions>(opts => opts.Execte())
                .WithParsed<DeleteOptions>(opts => opts.Execte())
                .WithParsed<ListOptions>(opts => opts.Execte())
                .WithNotParsed(err => Environment.Exit(1));
        }

        private static void RunOptionsAndReturnExitCode(MainOptions opts)
        {
            throw new NotImplementedException();
        }

        public static (string currentWorkDirectory, string projName) ParseArguments(string[] args)
        {
            if (args.Length < 1) FailedExit("The current work directory is required!");
            if (!Directory.Exists(args[0])) FailedExit("The current work directory isn't exists!");
            return (args[0], args.Length > 1 ? args[1].ToLowerInvariant() : null);
        }

        public static string GetTargetPath(string projectName, Dictionary<string,string> projects)
        {
            // Did not specific the project name.
            if (projectName == null) return projects["root"];
            // else
            var query = from project in projects
                        where project.Key.Contains(projectName ?? RootName)
                        select project;
            query.Distinct();
            switch (query.Count())
            {
                case 0:
                    FailedExit("Error: No such project!");
                    return null;
                case 1: return query.First().Value;
                default:
                    var project = query.Where(x => x.Key == projectName);
                    if (project.Count() == 1) return project.First().Value;
                    var matchedProject = 
                        string.Join('\n',
                            query.Select(
                                x => $"Project {x.Key}: {x.Value}"));
                    FailedExit($"Error: Multiple projects are matched!\n{matchedProject}");
                    return null;
            }
        }

        public static void FailedExit(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}
