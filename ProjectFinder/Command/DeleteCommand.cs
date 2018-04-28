using System;
using System.Collections.Generic;
using System.Linq;
using System.CommandLine;
using ProjectFinder.Manager;

namespace ProjectFinder.Command
{
    public class DeleteCommand : CommandBase
    {
        private IReadOnlyList<string> aliase;
        public IReadOnlyList<string> Aliases => aliase;

        private Dictionary<string,string> deletedAliases;
        private Dictionary<string,string> errorAliases;

        public override void Execute()
        {
            if (Aliases == null) ReportError("At least one alias should is required!");

            aliase = Aliases.ToHashSet().ToList() as IReadOnlyList<string>; // Avoid duplicate alias
            deletedAliases = new Dictionary<string,string>();
            errorAliases = new Dictionary<string,string>();

            foreach (var alias in Aliases)
                if (CacheManager.QueryDirectoryRecordByAlias(alias, out var record))
                    deletedAliases.Add(record.Name, record.Path);
                else
                    errorAliases.Add(alias, string.Empty);

            foreach (var alias in Aliases)
                CacheManager.RemoveDirectoryCache(alias);

            // Execute the change on files.
            // This part will not execute in test environment.
            if (Global.Configuration.CurrentEnvironment != EnvironmentMode.Test)
                CacheManager.SaveDirectoryCache();
            // Stop if in test environment
            else
            {
                Console.Error.WriteLine("In test environment!");
                return;
            }
        }

        protected override void ParseParameters(ArgumentSyntax syntax)
        {
            syntax.DefineParameterList("aliases", ref aliase, "The aliases of paths to be deleted.");
        }

        protected override void ShowResultInfo()
        {
            var statisticsInfo = $"\nTotal {Aliases.Count()} - {deletedAliases.Count} deleted, {errorAliases.Count} failed.\n";
            var deletedInfo = string.Join(string.Empty,
                deletedAliases.Select(record => $"  ✔︎ deleted - alias: {record.Key}. Path: \"{record.Value}\"\n"));
            var errorInfo = string.Join(string.Empty,
                errorAliases.Select(record => $"  ✘ error - alias: {record.Key}. The alias has not been saved.\n"));
            Console.Error.Write(statisticsInfo + deletedInfo + errorInfo);
        }
    }
}