/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MergeRequestReminder.Component.GitlabClient;
using MergeRequestReminder.Component.MacOSNotification;
using MergeRequestReminder.CustomException;
using MergeRequestReminder.Model;
using Newtonsoft.Json;

namespace MergeRequestReminder
{
    public class Engine
    {
        private LittleConfig config = new();
        private Cache cache = new();
        private DateTime? lastTimeNotificationWasShown = null;
        
        public async void StartChecking()
        {
            var running = true;
            do
            {
                ConsoleMessage.Clear();
                List<MergeRequest> openMergeRequests = null;
                try
                {
                    var gitlabClient = new GitlabClient();
                    openMergeRequests = await gitlabClient.GetOpenMergeRequestsThatNeedReviewByUser();
                }
                catch (ConfigValueException cvEx)
                {
                    ConsoleMessage.Error(cvEx.Message);
                    ConsoleMessage.Default("Press any key to exit");
                    running = false;
                    continue;
                }

                var sleepTimeOut = Convert.ToInt32(config.Values.CheckIntervalInMinutes) * 60 * 1000;
                if (openMergeRequests == null)
                {
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    ConsoleMessage.Error(
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
                    
                    MacOSNotification.Show("Open Merge Request Checker","Open Merge Requests assigned to you",$"You have {openMergeRequests.Count} open Merge Requests assigned to you","Frog");
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    ConsoleMessage.Info(
                        $"{notInCacheCount} waiting on your review 👀. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                }
                else
                {
                    var nextCheck =
                        DateTime.Now.Add(new TimeSpan(0, Convert.ToInt32(config.Values.CheckIntervalInMinutes), 0));
                    if (openMergeRequests.Count == 0)
                    {
                        ConsoleMessage.Success(
                            $"No open Merge Requests assigned to you 👏. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                    }
                    else
                    {
                        ConsoleMessage.Info(
                            $"{openMergeRequests.Count} waiting on your review 👀. Next check will be on {nextCheck:MM/dd/yyyy} at {nextCheck:hh:mm tt}");
                    }
                }

                ConsoleMessage.Warning("Press any key to quit");
                Thread.Sleep(sleepTimeOut);
            } while (running);

            /*{
                
            } //while (!cancellationToken.IsCancellationRequested);*/
        }
        /*private async Task<List<MergeRequest>> GetOpenMergeRequestsThatNeedReviewByUser()
        {
            if (config.Values.GitlabUserName == string.Empty)
            {
                throw new ConfigValueException("Configuration file is missing required value: GitLabUserName");
            }

            try
            {
                var url =
                    $"https://git.fullscript.io/api/v4/merge_requests?reviewer_username={config.Values.GitlabUserName}&state=opened&scope=all";
                var openMergeRequests = (await MakeHttpRequest<List<MergeRequest>>(url));
                var finalList = new List<MergeRequest>();

                if (!(openMergeRequests?.Count > 0)) return finalList;

                var headerPrinted = false;
                foreach (var openMergeRequest in openMergeRequests)
                {
                    var isApprovedByMe = await ApprovedByUser(openMergeRequest.ProjectId.ToString(),
                        openMergeRequest.Iid.ToString());
                    if (isApprovedByMe) continue;

                    if (!headerPrinted)
                    {
                        ConsoleMessage.Info("Merge Requests awaiting your review (⌘ double click link to open)");
                        headerPrinted = true;
                    }

                    finalList.Add(openMergeRequest);
                    ConsoleMessage.Default(openMergeRequest.WebUrl);
                }

                return finalList;
            }
            catch (ConfigValueException cve)
            {
                throw cve;
            }
            catch (Exception ex)
            {
                ConsoleMessage.AlertError($"An error occurred while attempting to fetch Merge Requests {ex.Message}",2);
            }

            return null;
        }
        private async Task<bool> ApprovedByUser(string projectId, string mergeRequestId)
        {
           var getApprovalUrl =
               $"https://git.fullscript.io/api/v4/projects/{projectId}/merge_requests/{mergeRequestId}/approvals";

           var approvalData = await MakeHttpRequest<Approval>(getApprovalUrl);

           var userName = config.Values.GitlabUserName;
           foreach (var user in approvalData.ApprovedBy)
           {
               if (user.User.Username == userName)
               {
                   return true;
               }
           }
           return false;
        }*/
        /*private static void ShowNotification(int openMergeRequests)
        {
            var command = $" -c \"osascript -e 'display notification \\\"Open Merge Request Checker\\\" with title \\\"Open Merge Requests assigned to you\\\" subtitle \\\"You have {openMergeRequests} open Merge Requests assigned to you.\\\" sound name \\\"Frog\\\"'";
            command = new string(command.Where(c => !char.IsControl(c)).ToArray());
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "/bin/bash",
                Arguments = command,
                CreateNoWindow = false,
            };
            process.StartInfo = startInfo;
            process.Start();
        }*/
       /* private async Task<T> MakeHttpRequest<T>(string url)
        {
            var httpClient = new HttpClient();
            var token = config.Values.PersonalAccessToken;
            if (token == string.Empty)
            {
                throw new ConfigValueException("Configuration file is missing required value: PersonalAccessToken");
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                ConsoleMessage.Error($"Call to {url} failed");
                return default;
            }

            var contents = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contents);
        }
    }
}*/