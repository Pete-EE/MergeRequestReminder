using System;
using System.IO;
using Newtonsoft.Json;

namespace MergeRequestReminder.Component.LittleConfig
{
    public class LittleConfig
    {

        private Config config = null;

        private string workingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "Library/MergeRequestReminderData");
        private FileSystemWatcher watcher;
        public LittleConfig()
        {
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }
            watcher = new FileSystemWatcher(workingDirectory, "*.json");
            watcher.Changed +=  (sender, args) =>
            {
                if (args.Name == "config.json")
                {
                    LoadConfigFromFile();
                    ConsoleMessage.ConsoleMessage.AlertInfo("Config changes will take place on next check", 3);
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

        public void ResetConfig()
        {
            config = new Config
            {
                CheckIntervalInMinutes = CaptureCheckInterval(),
                ForceNotificationIntervalInMinutes = CaptureForceInterval(),
                PersonalAccessToken = CaptureToken(),
                GitlabUserName = CaptureUserName()
            };
            WriteConfigFile();
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(config);
        }
        
        private void LoadConfigFromFile()
        {
            watcher.EnableRaisingEvents = false;
            var configFileLocation = Path.Combine(workingDirectory, "config.json");

            if (!File.Exists(configFileLocation))
            {
                ConsoleMessage.ConsoleMessage.AlertError("Config file was not found. A new one will be generated", 2);
                config = new Config
                {
                    CheckIntervalInMinutes = "5",
                    ForceNotificationIntervalInMinutes = "60",
                    PersonalAccessToken = CaptureToken(),
                    GitlabUserName = CaptureUserName()
                };
                WriteConfigFile();
            }

            var configContents = File.ReadAllText(configFileLocation);
            config = JsonConvert.DeserializeObject<Config>(configContents);
            ValidateConfig();
            watcher.EnableRaisingEvents = true;
        }

        public void WriteConfigFile()
        {
            watcher.EnableRaisingEvents = false;
            var configFileLocation = Path.Combine(workingDirectory, "config.json");
            File.WriteAllText(configFileLocation, JsonConvert.SerializeObject(config));
            watcher.EnableRaisingEvents = true;
        }

        private void ValidateConfig()
        {
            var change = false;
            
            if (!int.TryParse(config.CheckIntervalInMinutes, out var value) || value < 1)
            {
                ConsoleMessage.ConsoleMessage.AlertWarning("Check interval is too low. Defaulting to 5 minutes", 2);
                config.CheckIntervalInMinutes = "5";
                change = true;
            }

            if (!int.TryParse(config.ForceNotificationIntervalInMinutes, out value) || value < 1)
            {
                ConsoleMessage.ConsoleMessage.AlertWarning("Force notification interval is too low. Defaulting to 120 minutes", 2);
                config.ForceNotificationIntervalInMinutes = "120";
                change = true;
            }

            if (change)
            {
              WriteConfigFile();
            }
                
        }
        
        public string CaptureToken()
        {
            var isValid = false;
            var token = string.Empty;
            while (!isValid)
            {
                ConsoleMessage.ConsoleMessage.Clear();
                ConsoleMessage.ConsoleMessage.Default("Enter your personal access token");
                var enteredValue =  Console.ReadLine();

                if (enteredValue == string.Empty) continue;
                isValid = true;
                token = enteredValue;
            }

            return token;
        }
        
        public string CaptureUserName()
        {
            var isValid = false;
            var userName = string.Empty;

            while (!isValid)
            {
                ConsoleMessage.ConsoleMessage.Clear();
                ConsoleMessage.ConsoleMessage.Default("Enter your gitlab user name");
                var enteredValue = Console.ReadLine();

                if (enteredValue == string.Empty) continue;
                userName = enteredValue;
                isValid = true;
            }
            return userName;
        }
        
        public string CaptureCheckInterval()
        {
            var isValid = false;
            var checkInterval = string.Empty;

            while (!isValid)
            {
                ConsoleMessage.ConsoleMessage.Clear();
                ConsoleMessage.ConsoleMessage.Default("Enter how often you would like check gitlab for new Merge Requests");
                var enteredValue = Console.ReadLine();

                if (enteredValue == string.Empty) continue;
                if (int.TryParse(enteredValue, out var nEnteredValue))
                {
                    if (nEnteredValue < 1)
                    {
                        ConsoleMessage.ConsoleMessage.AlertError("Invalid value must be number >= 1", 2);
                        continue;
                    }
                }
                else
                {
                    ConsoleMessage.ConsoleMessage.AlertError("Invalid value must be number >= 1", 2);
                    continue;
                }
                checkInterval = enteredValue;
                isValid = true;
            }
            return checkInterval;
        }
        
        public string CaptureForceInterval()
        {
            var isValid = false;
            var forceNotification = string.Empty;

            while (!isValid)
            {
                ConsoleMessage.ConsoleMessage.Clear();
                ConsoleMessage.ConsoleMessage.Default("Enter how often to show all open merge request notification");
                var enteredValue = Console.ReadLine();

                if (enteredValue == string.Empty) continue;
                if (int.TryParse(enteredValue, out var nEnteredValue))
                {
                    if (nEnteredValue < 5)
                    {
                        ConsoleMessage.ConsoleMessage.AlertError("Invalid value must be number >= 5", 2);
                        continue;
                    }
                }
                else
                {
                    ConsoleMessage.ConsoleMessage.AlertError("Invalid value must be number > 1", 2);
                    continue;
                }
                forceNotification = enteredValue;
                isValid = true;
            }
            return forceNotification;
        }
    }

    public class Config
    {
        public string PersonalAccessToken { get; set; }
        public string GitlabUserName { get; set; }
        public string CheckIntervalInMinutes { get; set; }
        public string ForceNotificationIntervalInMinutes { get; set; }
    }
}