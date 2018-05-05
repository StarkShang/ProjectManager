using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectFinder.Model;
using ProjectFinder.Utilities;

namespace ProjectFinder.Manager
{
    public class SolutionManager
    {
        private FileCacheRecord SolutionFile;

        public static bool IsInSolution(string path, out SolutionManager manager)
        {
            manager = new SolutionManager();
            // Search cache file
            manager.SolutionFile = SearchCacheFiles(path);
            if (manager.SolutionFile != null) return true;
            
            manager.SolutionFile = TraverseParents(new DirectoryInfo(path));
            if (manager.SolutionFile != null) return true;

            manager = null;
            return false;
        }

        public string GetTargetPath(string alias)
        {
            // Get [alias:projectPath] dictionary
            var projectTable = SlnParser.ParseSolutionFile(SolutionFile);

            // Did not specific the project name.
            if (alias == null) return projectTable[Global.Configuration.SolutionRoot];
            // else
            var query = FuzzyMatch(alias, projectTable);
            
            // query.Distinct();
            switch (query.Count())
            {
                // No project is matched.
                case 0: return null;
                // A unique project is matched, which is expected.
                case 1: return query.First().Value;
                // Multiple projects are matched, query user where to go.
                default:
                    var projects = AccurateMatch(alias, projectTable);
                    if (projects.Count() == 1) return projects.First().Value;

                    var selection = InteractiveManager
                        .QueryUserForSelection(query,
                            x => $"alias: {x.Key}, path: {x.Value}");
                    return selection == 0 ? null : query.Values.ElementAt(selection-1);
            }
        }

        private Dictionary<string, string> FuzzyMatch(string condition, Dictionary<string, string> collection)
        {
            var query = from item in collection.ToArray()
                        where item.Key.Contains(condition)
                        select item;
            return new Dictionary<string,string>(query);
        }

        private Dictionary<string, string> AccurateMatch(string condition, Dictionary<string, string> collection)
        {
            var query = from item in collection
                   where item.Key == condition
                   select item;
            return new Dictionary<string,string>(query);
        }

        

        // The input parameter 'currentDirectory' must be a absolute path
        private static FileCacheRecord SearchCacheFiles(string currentDirectory)
        {
            // current directory  : ~/project/tool/bin
            // cached project0 : ~/project/project.sln
            // cached project1 : ~/project/tool/project.sln
            // cached project2 : ~/Document/project.sln
            // cached project0 and cached project1 should be matched
            // So the longest directory parth is selected
            var query = from sln in CacheManager.CachedFiles
                        where currentDirectory.StartsWith(sln.Path, StringComparison.InvariantCultureIgnoreCase)
                        select (sln);
            switch (query.Count())
            {
                case 0: return null;
                case 1: return query.First();
                default: return GetClosestFile(query);
            }
        }

        private static FileCacheRecord TraverseParents(DirectoryInfo current)
        {
            if (current == null) return null;

            var slnFileInfos = current.GetFiles("*.sln", SearchOption.TopDirectoryOnly);
            switch (slnFileInfos.Count())
            {
                case 0: return TraverseParents(current.Parent);
                case 1:
                    var slnFileInfo = slnFileInfos.First();
                    return RecordToCacheFile(new FileCacheRecord
                    {
                        FilePath = slnFileInfo.FullName,
                        Name = slnFileInfo.Name,
                        Path = slnFileInfo.DirectoryName
                    });
                default: throw new Exception("Error: Multiple sulotion files exists!");
            }
        }


        // current directory  : ~/project/tool/bin
        // matched directory1 : ~/project
        // matched directory2 : ~/project/tool
        // matcged directory2 should be matched
        // So the longest directory parth is selected
        private static FileCacheRecord GetClosestFile(IEnumerable<FileCacheRecord> matcedFileInfos) =>
            matcedFileInfos.OrderByDescending(x => x.Path.Length).First();

        private static FileCacheRecord RecordToCacheFile(FileCacheRecord record)
        {
            CacheManager.AddFileCacheRecord(record);
            CacheManager.SaveFileCache();
            return record;
        }
    }
}