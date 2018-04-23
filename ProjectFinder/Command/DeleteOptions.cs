using CommandLine;

namespace ProjectFinder.Command
{
    [Verb("delete", HelpText = "Delete a record in saved list by alias. The whole alias should be given.")]
    public class DeleteOptions : ICommand
    {
        public void Execte()
        {
            throw new System.NotImplementedException();
        }
    }
}