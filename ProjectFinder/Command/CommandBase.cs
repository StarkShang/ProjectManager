using System;
using System.CommandLine;
using System.Linq;
using System.Text;

namespace ProjectFinder.Command
{
    public abstract class CommandBase
    {
        public string HelpInfo { get; set; }
        public CommandBase ActiveCommand { get; set; }
        public string[] UnparsedArguments { get; set; }

        protected virtual void ParseCommands(ArgumentSyntax syntax) { }
        protected virtual void ParseParameters(ArgumentSyntax syntax) { }
        protected void ParseArguments(string[] args)
        {
            try
            {
                // Parse args
                ArgumentSyntax.Parse(args, syntax =>
                {
                    syntax.HandleErrors = false;
                    syntax.ErrorOnUnexpectedArguments = false;

                    ParseCommands(syntax);

                    ActiveCommand = syntax.ActiveCommand?.Value as CommandBase;
                    UnparsedArguments = syntax.RemainingArguments.ToArray();
                    
                    HelpInfo = syntax.Commands.Count == 0 ? string.Empty : syntax.GetHelpText(Console.WindowWidth - 2);
                });
            }
            catch (ArgumentSyntaxException) { }

            if (ActiveCommand == null)
            {
                try
                {
                    var result = ArgumentSyntax.Parse(args, syntax =>
                    {
                        syntax.HandleErrors = false;

                        ParseParameters(syntax);
                        HelpInfo += syntax.GetHelpText(Console.WindowWidth - 2);
                    });
                }
                catch (ArgumentSyntaxException e)
                {
                    ReportError($"error: {e.Message}");
                }
            }
        }

        public abstract void Execute();
        public void Execute(string[] args)
        {
            ParseArguments(args);

            // If subcommand is matched, execute the command.
            if (ActiveCommand != null)
                ActiveCommand.Execute(UnparsedArguments);
            else
                Execute();

            ShowResultInfo();
        }

        protected void ReportError(string message)
        {
            Console.Error.WriteLine($"\n{message}\n\n{HelpInfo}");
        }

        protected abstract void ShowResultInfo();
    }
}