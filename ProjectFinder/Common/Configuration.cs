using System.Collections.Generic;

namespace ProjectFinder
{
    public enum EnvironmentMode
    {
        Test,
        Development,
        Release
    }
    public class Configuration
    {
        public EnvironmentMode CurrentEnvironment { get; set; }
        public string DirectoryCachePath { get; set; }
        public string FileCachePath { get; set; }
        public string SolutionRoot { get; set; }
    }
}