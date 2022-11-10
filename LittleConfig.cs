/*using System.IO;
using System.Reflection;
using MergeRequestReminder.CustomException;
using Newtonsoft.Json;

namespace MergeRequestReminder;

public class LittleConfig
{
    private Config config = null;
    private string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private FileSystemWatcher watcher;

    public LittleConfig()
    {
        watcher = new FileSystemWatcher(workingDirectory,"*.json");
        watcher.Changed += (sender, args) =>
        {
            if (args.Name == "config.json")
            {
                LoadConfigFromFile();
                ConsoleMessage.AlertInfo("Config changes will take place on next check",3);
            }
        };
        watcher.EnableRaisingEvents = true;
        LoadConfigFromFile();
    }
    public Config Values
    {
        get
        {
            if (config != null) return config;
            LoadConfigFromFile();
            return config;    
        }
    }
    private void LoadConfigFromFile()
    {
        var configFileLocation = Path.Combine(workingDirectory, "config.json");

        if (!File.Exists(configFileLocation))
        {
            InitConfig();
            return;
        }

        var configContents = File.ReadAllText(configFileLocation);
        config = JsonConvert.DeserializeObject<Config>(configContents);
        ValidateConfig();
    }
    private void ValidateConfig()
    {
        var value = 0;
        if (!int.TryParse(config.CheckIntervalInMinutes, out value) || value < 1)
        {
            ConsoleMessage.AlertWarning("Check interval is too low. Defaulting to 5 minutes", 2);
            config.CheckIntervalInMinutes = "5";
        }

        if (int.TryParse(config.ForceNotificationIntervalInMinutes, out value) || value < 1)
        {
            ConsoleMessage.AlertWarning("Force notification interval is too low. Defaulting to 120 minutes", 2);
            config.ForceNotificationIntervalInMinutes = "120";
        }
    }
    private void InitConfig()
    {
        File.WriteAllText(Path.Combine(workingDirectory,"config.json"), JsonConvert.SerializeObject( new Config
        {
            GitlabUserName = "",
            PersonalAccessToken = "",
            CheckIntervalInMinutes = "5",
            ForceNotificationIntervalInMinutes = "120"
        }));
        throw new ConfigNotFoundException(
            "Config file was generated but you will need to enter your gitlab username and personal access token");
    }
}
public class Config
{
    public string PersonalAccessToken { get; set; }
    public string GitlabUserName { get; set; }
    public string CheckIntervalInMinutes { get; set; }
    public string ForceNotificationIntervalInMinutes { get; set; }
}*/