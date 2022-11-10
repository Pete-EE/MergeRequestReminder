/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MergeRequestReminder;

public class Cache
{
    private string cachePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    private FileSystemWatcher watcher;
    private List<string> cacheList = new(); 
    public Cache()
    {
        watcher = new FileSystemWatcher(cachePath,"*.ds");
        watcher.Changed += (sender, args) =>
        {
            if (args.Name == "cache.ds")
            {
                LoadCacheFromFile();
                ConsoleMessage.AlertWarning("External change to cache. Reloading",2);
            }
        };
        watcher.EnableRaisingEvents = true;
        LoadCacheFromFile();
    }

    public bool TryAddToCache(string id)
    {
        if (InCache(id)) return false;
        AddToCache(id);
        return true;

    }

    public bool TryRemoveFromCache(string id)
    {
        if (!InCache(id)) return false;
        RemoveFromCache(id);
        return true;
    }

    public void ReplaceCache(List<string> newCache)
    {
        cacheList = newCache;
        SaveToFile();
    }
    public bool InCache(string id)
    {
        return cacheList.Contains(id);
    }
    private void AddToCache(string id)
    {
        cacheList.Add(id);
        cacheList.Sort();
        SaveToFile();
    }

    private void RemoveFromCache(string id)
    {
        cacheList.Remove(id);
        cacheList.Sort();
        SaveToFile();
        
    }

    private void SaveToFile()
    {
        var configFilePath = Path.Combine(cachePath, "cache.ds");
        watcher.EnableRaisingEvents = false;
        var contents = String.Join(",", cacheList.ToArray());
        File.WriteAllText(configFilePath, contents);
        watcher.EnableRaisingEvents = true;
    }
    private void LoadCacheFromFile()
    {
        var configFilePath = Path.Combine(cachePath, "cache.ds");

        if (!File.Exists(configFilePath))
        {
            SaveToFile();
        }
        cacheList = File.ReadAllText(Path.Combine(cachePath, "cache.ds")).Split(',').ToList();
    }

}*/