using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectTable = System.Collections.Generic.Dictionary<string, string>;

namespace ProjectFinder
{
    public class SolutionManager
    {
        public static ProjectFile GetSolutionFile(string currentDirectory)
        {
            // Search cache file
            var rst = SearchCacheFile(currentDirectory);
            if (rst != null) return rst;
            return TraverseParents(new DirectoryInfo(currentDirectory));
        }

        private static ProjectFile SearchCacheFile(string currentDirectory)
        {
            // current directory  : ~/project/tool/bin
            // cached project0 : ~/project/project.sln
            // cached project1 : ~/project/tool/project.sln
            // cached project2 : ~/Document/project.sln
            // cached project0 and cached project1 should be matched
            // So the longest directory parth is selected
            var query = from sln in GlobalObject.SolutionTable
                        where new DirectoryInfo(currentDirectory).FullName.StartsWith(sln.Path, StringComparison.InvariantCultureIgnoreCase)
                        select (sln);
            switch (query.Count())
            {
                case 0: return null;
                case 1: return query.First();
                default: return GetClosestFile(query);
            }
        }

        private static ProjectFile TraverseParents(DirectoryInfo current)
        {
            if (current == null) return null;

            var slnFileInfos = current.GetFiles("*.sln", SearchOption.TopDirectoryOnly);
            switch (slnFileInfos.Count())
            {
                case 0: return TraverseParents(current.Parent);
                case 1:
                    var slnFileInfo = slnFileInfos.First();
                    return RecordToCacheFile(new ProjectFile
                    {
                        Id = Guid.NewGuid(),
                        Name = slnFileInfo.Name,
                        FullName = slnFileInfo.FullName,
                        Path = slnFileInfo.DirectoryName
                    });
                default: throw new Exception("Error: Multiple sulotion files exists!");
            }
        }

        private static ProjectFile GetClosestFile(IEnumerable<ProjectFile> matcedFileInfos)
        {
            // current directory  : ~/project/tool/bin
            // matched directory1 : ~/project
            // matched directory2 : ~/project/tool
            // matcged directory2 should be matched
            // So the longest directory parth is selected
            return matcedFileInfos.OrderByDescending(x => x.Path.Length).First();
        }

        private static ProjectFile RecordToCacheFile(ProjectFile slnFile)
        {
            GlobalObject.CachedSolution(slnFile);
            return slnFile;
        }
    }
}
