using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MergeRequestReminder.Component.Cache;
using MergeRequestReminder.CustomException;
using MergeRequestReminder.Model;

namespace MergeRequestReminder.Component.Engine
{
    public class Engine
    {
        private LittleConfig.LittleConfig config = new();
        private Cache<string> cache = new();
        private DateTime? lastTimeNotificationWasShown = null;

        public async void StartChecking()
        {
            var running = true;
            do
            {
                ConsoleMessage.ConsoleMessage.Clear();
                List<MergeRequest> openMergeRequests = null;
                try
                {
                    var gitlabClient = new GitlabClient.GitlabClient();
                    openMergeRequests = await gitlabClient.GetOpenMergeRequestsThatNeedReviewByUser();
                }
                catch (ConfigValueException cvEx)
                {
                    ConsoleMessage.ConsoleMessage.Error(cvEx.Message);
                    ConsoleMessage.ConsoleMessage.Default("Press any key to exit");
                    running = false;
                    continue;
                }

                var sleepTimeOut = Convert.ToInt32(config.Values.CheckIntervalInMinutes) * 60 * 1000;
                if (openMergeRequests == null)
                {
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    ConsoleMessage.ConsoleMessage.Error(
                        $"An error occurred while fetching Merge Requests. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                    Thread.Sleep(sleepTimeOut);
                    continue;

                }

                TimeSpan? timeSinceLastNotificationShown = null;

                var notInCacheCount = 0;
                if (lastTimeNotificationWasShown != null)
                {
                    timeSinceLastNotificationShown = DateTime.Now.Subtract(lastTimeNotificationWasShown.Value);

                    var didParse = int.TryParse(config.Values.ForceNotificationIntervalInMinutes,
                        out var forceCheckInterval);

                    if (timeSinceLastNotificationShown.Value.Minutes < forceCheckInterval || !didParse)
                    {
                        notInCacheCount = openMergeRequests.Where(item => !cache.InCache(item.Iid.ToString())).ToList()
                            .Count;
                    }
                    else
                    {
                        notInCacheCount =
                            openMergeRequests
                                .Count; //force the notification for all open MergeRequests assigned to user
                    }
                }
                else
                {
                    notInCacheCount = openMergeRequests.Where(item => !cache.InCache(item.Iid.ToString())).ToList()
                        .Count;
                }

                cache.ReplaceCache(openMergeRequests.Select(item => item.Iid.ToString()).ToList());

                if (notInCacheCount > 0)
                {
                    lastTimeNotificationWasShown = DateTime.Now;

                    await MacOSNotification.MacOSNotification.Show("Open Merge Request Checker", "Open Merge Requests assigned to you",
                        $"You have {openMergeRequests.Count} open Merge Requests assigned to you", "Frog");
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    ConsoleMessage.ConsoleMessage.Info(
                        $"{notInCacheCount} waiting on your review 👀. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                }
                else
                {
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    if (openMergeRequests.Count == 0)
                    {
                        ConsoleMessage.ConsoleMessage.Success(
                            $"No open Merge Requests assigned to you 👏. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                    }
                    else
                    {
                        ConsoleMessage.ConsoleMessage.Info(
                            $"{openMergeRequests.Count} waiting on your review 👀. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                    }
                }

                ConsoleMessage.ConsoleMessage.Warning("Press any key to quit");
                Thread.Sleep(sleepTimeOut);
            } while (running);
        }

        public void ProcessCommands(string[] args)
        {
            var configUpdated = false;
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "-reset-config":
                        config.ResetConfig();
                        break;
                    case "-view-config":
                        Console.WriteLine("Current config");
                        Console.Write(config.ToJsonString());
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;
                    case "-edit-config-pat":
                        config.Values.PersonalAccessToken = config.CaptureToken();
                        configUpdated = true;
                        break;
                    case "-edit-config-username":
                        config.Values.GitlabUserName = config.CaptureUserName();
                        configUpdated = true;
                        break;
                    case "-edit-config-checkinterval":
                        config.Values.CheckIntervalInMinutes = config.CaptureCheckInterval();
                        configUpdated = true;
                        break;
                    case "-edit-config-forcenotificationinterval":
                        config.Values.ForceNotificationIntervalInMinutes = config.CaptureForceInterval();
                        configUpdated = true;
                        break;
                    case "-cache-clear":
                        cache.Clear();
                        break;
                    case "-cache-view":
                        Console.WriteLine("Items in cache");
                        Console.Write(cache.ToJsonString());
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;
                    case "-log-output":
                        //verbose but good for tracking errors that show as alerts in the console
                        ConsoleMessage.ConsoleMessage.EnableOutputLogging = true;
                        break;
                }
            }
            if (configUpdated)
            {
                config.WriteConfigFile();
            }
        }

        public void CommandPrompt()
        {
            Console.WriteLine("Enter command: ");
            var command = Console.ReadLine();
            if (command == null) return;
            
            if (command[0] != '-')
                command = "-" + command;
            ConsoleMessage.ConsoleMessage.SaveBuffer();
            ProcessCommands(new string[]{command});
            ConsoleMessage.ConsoleMessage.RestoreBuffer();
        }
    }
}
