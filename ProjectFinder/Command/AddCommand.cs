using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ProjectFinder.Manager;
using ProjectFinder.Model;
using System.CommandLine;

namespace ProjectFinder.Command
{
    public class AddCommand : CommandBase
    {
        // If not specific, it will be null
        private string path;
        public string Path => path;
        private IReadOnlyList<string> aliases;
        public IReadOnlyList<string> Aliases => aliases;
        private List<string> addedAliases;
        private List<string> errorAliases;

        public override void Execute()
        {
            var directory = new DirectoryInfo(Path ?? Environment.CurrentDirectory);
            path = directory.FullName;
            
            if (Aliases == null)
                aliases = new List<string> {
                    directory.Name
                } as IReadOnlyList<string>;
            else
                aliases = Aliases.ToHashSet().ToList() as IReadOnlyList<string>; // Avoid duplicate alias
            
            addedAliases = new List<string>(Aliases.Count());
            errorAliases = new List<string>(Aliases.Count());

            foreach (var alias in Aliases)
                if (CacheManager.IsDirectoryAliasExist(alias))
                    errorAliases.Add(alias);
                else
                    addedAliases.Add(alias);
            
            foreach (var alias in addedAliases)
                CacheManager.AddDirectoryCacheRecord(new DirectoryCacheRecord{
                    Name = alias,
                    Path = Path,
                });

            // Execute the change on files.
            // This part will not execute in test environment.
            if (Global.Configuration.CurrentEnvironment != EnvironmentMode.Test)
                CacheManager.SaveDirectoryCache();
            // Stop if in test environment
            else
            {
                Console.Error.WriteLine("Test end!");
                return;
            }
        }

        protected override void ParseParameters(ArgumentSyntax syntax)
        {
            syntax.DefineOption("p|path", ref path, "The directory path to be saved.");
            syntax.DefineParameterList("aliases", ref aliases, "The aliases of the path to be saved.");
        }

        protected override void ShowResultInfo()
        {
            var statisticsInfo = $"\nDirectory \"{Path}\" is saved!\nTotal {Aliases.Count()} aliases - {addedAliases.Count} add, {errorAliases.Count} are failed!\n";
            var addedInfo = string.Join(string.Empty,
                addedAliases.Select(alias => $"  ✔︎ added - alias: {alias}.\n"));
            var errorInfo = string.Join(string.Empty,
                errorAliases.Select(alias => $"  ✘ error - alias: {alias}. This alias is already exist or same as build-in keywords.\n"));
            Console.Error.Write(statisticsInfo + addedInfo + errorInfo);
        }
    }
}