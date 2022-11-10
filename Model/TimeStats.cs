using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class TimeStats
{
    [JsonProperty("time_estimate")]
    public int TimeEstimate { get; set; }

    [JsonProperty("total_time_spent")]
    public int TotalTimeSpent { get; set; }

    [JsonProperty("human_time_estimate")]
    public object HumanTimeEstimate { get; set; }

    [JsonProperty("human_total_time_spent")]
    public object HumanTotalTimeSpent { get; set; }
}