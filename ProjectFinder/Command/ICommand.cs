namespace ProjectFinder.Command
{
    public interface ICommand
    {
        void ParseArguments(string[] args);
        void Execte(string[] args);
    }
}