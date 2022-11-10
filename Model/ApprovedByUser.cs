using System.Collections.Generic;
using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class ApprovedByUser
{
    [JsonProperty("user")] 
    public GitlabUser User { get; set; }
}