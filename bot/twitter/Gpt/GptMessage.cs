using System.Text.Json.Serialization;

namespace twitter.Gpt;

public record GptMessage
{
    [JsonPropertyName("role")]
    public string? Role { get; set; } = "user";
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    public GptMessage(string content)
    {
        Content = content;
    }
}