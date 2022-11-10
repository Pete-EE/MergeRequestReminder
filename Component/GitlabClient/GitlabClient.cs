using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MergeRequestReminder.CustomException;
using MergeRequestReminder.Model;
using Newtonsoft.Json;

namespace MergeRequestReminder.Component.GitlabClient
{
    public class GitlabClient
    {
        private LittleConfig.LittleConfig config = new();

        public async Task<List<MergeRequest>> GetOpenMergeRequestsThatNeedReviewByUser()
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
                        ConsoleMessage.ConsoleMessage.Info("Merge Requests awaiting your review (⌘ double click link to open)");
                        headerPrinted = true;
                    }

                    finalList.Add(openMergeRequest);
                    ConsoleMessage.ConsoleMessage.Default(openMergeRequest.WebUrl);
                }

                return finalList;
            }
            catch (ConfigValueException cve)
            {
                throw cve;
            }
            catch (Exception ex)
            {
                ConsoleMessage.ConsoleMessage.AlertError($"An error occurred while attempting to fetch Merge Requests {ex.Message}",
                    2);
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
        }

        private async Task<T> MakeHttpRequest<T>(string url)
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
                ConsoleMessage.ConsoleMessage.Error($"Call to {url} failed");
                if (res.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ConsoleMessage.ConsoleMessage.Error("Check that your username and personal access token are correct");
                }
                return default;
            }

            var contents = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contents);
        }
    }
}