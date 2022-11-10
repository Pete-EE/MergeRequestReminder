using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class GitlabUser
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("web_url")]
    public string WebUrl { get; set; }
}