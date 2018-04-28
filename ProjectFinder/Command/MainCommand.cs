using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using ProjectFinder.Manager;
using System.IO;

namespace ProjectFinder.Command
{
    public class MainCommand : CommandBase
    {
        public AddCommand AddCommand { get; } = new AddCommand();
        public DeleteCommand DeleteCommand { get; } = new DeleteCommand();
        public ListCommand ListCommand { get; } = new ListCommand();
        private string alias;
        public string Alias => alias;
        private bool globalSearch;
        public bool GlobalSearch => globalSearch;

        public sealed override void Execute()
        {
            // Solution search mode
            // In a solution without -g option
            if (!GlobalSearch && SolutionManager.IsInSolution(Directory.GetCurrentDirectory(), out var manager))
            {
                var path = manager.GetTargetPath(Alias);
                if (path != null)
                {
                    Console.WriteLine(path);
                    Environment.Exit(0);
                }
                else ReportError("No such project!");
            }
            // Global search mode :
            //   - Out of a solution
            //   - In a solution with -g option
            else
            {
                if (Alias == null) ReportError("At least one alias is required in global search mode");
                else if(CacheManager.CachedDirectories.ContainsKey(Alias))
                {
                    Console.WriteLine(CacheManager.CachedDirectories[Alias].Path);
                    Environment.Exit(0);
                }
                else ReportError("Unrecognized aliases.");
            }
        }

        protected override void ParseCommands(ArgumentSyntax syntax)
        {
            syntax.DefineCommand("add", new AddCommand()).Help = "Save a directory path as the aliases.";
            syntax.DefineCommand("delete", new DeleteCommand()).Help = "Delete directories by their aliases. The full names of the alias are required.";
            syntax.DefineCommand("list", new ListCommand()).Help = "List all saved directories.";
        }
        protected override void ParseParameters(ArgumentSyntax syntax)
        {
            syntax.DefineOption("g|global", ref globalSearch, "Force global search");
            syntax.DefineParameter("alias", ref alias, "The alias of a directory");
        }

        protected override void ShowResultInfo() { }
    }
}