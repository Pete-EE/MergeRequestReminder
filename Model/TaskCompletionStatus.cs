using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class TaskCompletionStatus
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("completed_count")]
    public int CompletedCount { get; set; }
}