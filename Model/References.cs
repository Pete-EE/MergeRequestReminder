using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class References
{
    [JsonProperty("short")]
    public string Short { get; set; }

    [JsonProperty("relative")]
    public string Relative { get; set; }

    [JsonProperty("full")]
    public string Full { get; set; }
}