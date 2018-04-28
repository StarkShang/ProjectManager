using System;
using System.Linq;
using ProjectFinder.Manager;

namespace ProjectFinder.Command
{
    public class ListCommand : CommandBase
    {
        public override void Execute() { }

        protected override void ShowResultInfo()
        {
            var statisticsInfo = $"\nTotal {CacheManager.CachedDirectories.Count} aliases are found.\n";
            var listInfo = string.Join(string.Empty,
                CacheManager.CachedDirectories
                    .Select(i => $"  ❖ {i.Value.Name} : {i.Value.Path}\n"));
            
            Console.Error.Write(statisticsInfo + listInfo);
        }
    }
}