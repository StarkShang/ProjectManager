using CommandLine;
using CommandLine.Text;

namespace ProjectFinder.Command
{
    [Verb("list", HelpText = "List changes to the repository.")]
    public class ListOptions : ICommand
    {
        public void Execte()
        {
            throw new System.NotImplementedException();
        }
    }
}