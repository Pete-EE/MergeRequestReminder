using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MergeRequestReminder.Component.Cache;

public class Cache<T>
{
    private string cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "Library/MergeRequestReminderData");

    private FileSystemWatcher watcher;
    private List<T> cacheList = new(); 
    public Cache()
    {
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }
        watcher = new FileSystemWatcher(cachePath,"*.ds");
        watcher.Changed += (sender, args) =>
        {
            if (args.Name != "cache.ds") return;
            LoadCacheFromFile();
            ConsoleMessage.ConsoleMessage.AlertWarning("External change to cache. Reloading",2);
        };
        watcher.EnableRaisingEvents = true;
        LoadCacheFromFile();
    }

    public bool TryAddToCache(T id)
    {
        if (InCache(id)) return false;
        AddToCache(id);
        return true;

    }

    public bool TryRemoveFromCache(T id)
    {
        if (!InCache(id)) return false;
        RemoveFromCache(id);
        return true;
    }
    public void Clear()
    {
        ReplaceCache(new List<T>());
    }

    public string ToJsonString()
    {
        return JsonConvert.SerializeObject(cacheList);
    }
    public void ReplaceCache(List<T> newCache)
    {
        cacheList = newCache;
        SaveToFile();
    }
    public bool InCache(T id)
    {
        return cacheList.Contains(id);
    }
    private void AddToCache(T id)
    {
        cacheList.Add(id);
        cacheList.Sort();
        SaveToFile();
    }

    private void RemoveFromCache(T id)
    {
        cacheList.Remove(id);
        cacheList.Sort();
        SaveToFile();
        
    }

    private void SaveToFile()
    {
        var configFilePath = Path.Combine(cachePath, "cache.ds");
        watcher.EnableRaisingEvents = false;
        File.WriteAllText(configFilePath, JsonConvert.SerializeObject(cacheList));
        watcher.EnableRaisingEvents = true;
    }
    private void LoadCacheFromFile()
    {
        var configFilePath = Path.Combine(cachePath, "cache.ds");

        if (!File.Exists(configFilePath))
        {
            SaveToFile();
        }

        cacheList = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(Path.Combine(cachePath, "cache.ds")));
    }
}