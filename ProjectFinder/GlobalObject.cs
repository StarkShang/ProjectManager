using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ProjectFinder
{
    internal static class GlobalObject
    {
        static string cachedFile = @"SolutionList.json";

        private static readonly ICollection<ProjectFile> solutionTable;
        internal static ICollection<ProjectFile> SolutionTable => solutionTable;

        public static void CachedSolution(ProjectFile project)
        {
            solutionTable.Add(project);
            var json = JsonConvert.SerializeObject(solutionTable);
            File.WriteAllText(cachedFile, json);
        }

        static GlobalObject()
        {
            solutionTable = JsonConvert.DeserializeObject<ICollection<ProjectFile>>(value: File.ReadAllText(cachedFile));
        }
    }
}
