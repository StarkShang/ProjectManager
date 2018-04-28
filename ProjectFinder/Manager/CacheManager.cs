using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectFinder.Model;

namespace ProjectFinder.Manager
{
    internal static class CacheManager
    {
        private static readonly ICollection<FileCacheRecord> cachedFiles;
        internal static ICollection<FileCacheRecord> CachedFiles => cachedFiles;
        public static void AddFileCacheRecord(FileCacheRecord record) => cachedFiles.Add(record);
        public static void SaveFileCache() =>
            File.WriteAllText(
                Global.Configuration.FileCachePath, JsonConvert.SerializeObject(cachedFiles));


        private static readonly Dictionary<string, DirectoryCacheRecord> cachedDirectories;
        internal static Dictionary<string, DirectoryCacheRecord> CachedDirectories => cachedDirectories;
        public static void AddDirectoryCacheRecord(DirectoryCacheRecord record)
        {
            try
            {
                cachedDirectories.Add(record.Name, record);
            }
            catch (Exception err)
            {
                if(Global.Configuration.CurrentEnvironment == EnvironmentMode.Development ||
                   Global.Configuration.CurrentEnvironment == EnvironmentMode.Test)
                    Console.Error.WriteLine($"\n{err.Message}\n");
                else
                    Console.Error.WriteLine($"\nThe alias {record.Name} is already exist!\n");
            }
        }
        public static void RemoveDirectoryCache(string name) => cachedDirectories.Remove(name);
        public static bool IsDirectoryAliasExist(string alias) => CachedDirectories.ContainsKey(alias);
        public static bool QueryDirectoryRecordByAlias(string alias, out DirectoryCacheRecord record)
        {
            if (CachedDirectories.ContainsKey(alias))
            {
                record = CachedDirectories[alias];
                return true;
            }
            else
            {
                record = null;
                return false;
            }
        }
        public static void SaveDirectoryCache() =>
            File.WriteAllText(
                AppDomain.CurrentDomain.BaseDirectory + Global.Configuration.DirectoryCachePath,
                JsonConvert.SerializeObject(cachedDirectories));

        static CacheManager()
        {
            cachedFiles = JsonConvert.DeserializeObject<ICollection<FileCacheRecord>>(value: File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + Global.Configuration.FileCachePath));
            cachedDirectories = JsonConvert.DeserializeObject<Dictionary<string, DirectoryCacheRecord>>(value: File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + Global.Configuration.DirectoryCachePath));
        }
    }
}
