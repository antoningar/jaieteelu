using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace twitter.Gpt;

public static class GptService
{
    public static async Task<string[]> BuildAnecdotesAsync(GptOptions gptOption, string[] anecdotes)
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {gptOption.Key}");

        HttpRequestMessage requestMessage = BuildRequestMessage(gptOption, anecdotes.ToList());

        HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
        
        string responseStr = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"openapi call return {responseMessage.StatusCode}");
        }

        return ReadResponse(responseStr);
    }

    private static HttpRequestMessage BuildRequestMessage(GptOptions gptOption, List<string> anecdotes)
    {
        const string URL = "https://api.openai.com/v1/chat/completions";
        GptRequest request = new(gptOption.Model, anecdotes);
        string jsonBody = JsonSerializer.Serialize(request);
        Console.WriteLine($"GPT prompt : {string.Join(" | ", anecdotes)}");
        
        HttpRequestMessage requestMessage = new(HttpMethod.Post, URL);
        requestMessage.Content = new StringContent(jsonBody);
        requestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

        return requestMessage;
    }

    private static string[] ReadResponse(string responseStr)
    {
        JsonNode nodeResponse = JsonSerializer.Deserialize<JsonNode>(responseStr)!;

        //logs tokens used
        int promptTokens = nodeResponse["usage"]!["prompt_tokens"]!.GetValue<int>();
        int completionTokens = nodeResponse["usage"]!["completion_tokens"]!.GetValue<int>();
        Console.WriteLine($"{promptTokens} prompt tokens, {completionTokens} completion tokens");

        string[] responses = nodeResponse["choices"]![0]!["message"]!["content"]!.GetValue<string>().Split(" :: ");
        Console.WriteLine($"GPT Response : {string.Join(" | ", responses)}");
        
        return responses;
    }
}