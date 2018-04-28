using System;
using System.CommandLine;
using Microsoft.Extensions.Configuration;
using ProjectFinder.Command;
using ProjectFinder.Manager;

namespace ProjectFinder
{
    public class Program
    {
        const string SolutionFileSuffix = "sln";
        const string RootName = "root";

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appsettings.json", optional: true)
                .AddJsonFile(
                    $"{Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT")}.appsettings.json", optional: true)
                .Build();
            Global.Configuration = configuration.Get<Configuration>();

            // CommandManager.ExecuteCommand(args);
            new MainCommand().Execute(args);

            Environment.Exit(1);
        }
    }
}
