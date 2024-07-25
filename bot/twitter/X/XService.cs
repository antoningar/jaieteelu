using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OAuth;

namespace twitter.X;

public static class XService
{
    public static async Task PostDeputy(XOptions options, IEnumerable<string> parts, long mediaId)
    {
        string[] contents = parts as string[] ?? parts.ToArray();
        string result = await PostTweet(options, BuildRequestWithMedia(contents[0], mediaId));
        
        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }
        
        string id = GetTweetId(result);

        foreach (string content in contents[1..])
        {
            id = GetTweetId(await PostTweet(options, BuildRequest(content, id)));
        }
    }

    private static string BuildRequestWithMedia(string content, long mediaId)
    {
        return $"{{\"text\": \"{content}\",\"media\": {{\"media_ids\": [\"{mediaId}\"]}}}}";
    }

    private static string BuildRequest(string content, string replyId)
    {
        return $"{{\"text\": \"{content}\", \"reply\": {{\"in_reply_to_tweet_id\": \"{replyId}\"}}}}";
    }

    public static async Task<long> UploadPicturesAsync(XOptions options, string filename)
    {
        const string URL = "https://upload.twitter.com/1.1/media/upload.json?media_category=TWEET_IMAGE";
        const string MEDIA_TYPE = "multipart/form-data";
        
        using MultipartFormDataContent content = new();
        content.Headers.ContentType!.MediaType = MEDIA_TYPE;
        
        Stream fileStream = File.OpenRead(filename);
        content.Add(new StreamContent(fileStream), "media", filename);
        
        OAuthRequest oauth = new()
        {
            Method = HttpMethod.Post.ToString(),
            Type = OAuthRequestType.ProtectedResource,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = options.ConsumerKey,
            ConsumerSecret = options.ConsumerSecret,
            Token = options.AccessToken,
            TokenSecret = options.AccessTokenSecret,
            RequestUrl = URL
        };

        string? authHeader = oauth.GetAuthorizationHeader();

        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MEDIA_TYPE));
        
        HttpResponseMessage response = await httpClient.PostAsync(URL, content);
        string responseStr = await response.Content.ReadAsStringAsync();
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"X api call return {response.StatusCode}");
        }

        return GetMediaId(responseStr);
    }
    
    private static async Task<string> PostTweet(XOptions options, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return string.Empty;
        }
        
        const string URL = "https://api.twitter.com/2/tweets";
        OAuthRequest oauth = new()
        {
            Method = HttpMethod.Post.ToString(),
            Type = OAuthRequestType.ProtectedResource,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = options.ConsumerKey,
            ConsumerSecret = options.ConsumerSecret,
            Token = options.AccessToken,
            TokenSecret = options.AccessTokenSecret,
            RequestUrl = URL
        };

        string? authHeader = oauth.GetAuthorizationHeader();

        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

        HttpRequestMessage request = new (HttpMethod.Post, URL);
        request.Content = new StringContent(body.Replace("\n", ""), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.SendAsync(request);
        string responseStr = await response.Content.ReadAsStringAsync();
        
        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new Exception($"X api call return {response.StatusCode}");
        }

        return responseStr;
    }

    private static string GetTweetId(string result)
    {
        JsonNode nodeResponse = JsonSerializer.Deserialize<JsonNode>(result)!;
        return nodeResponse["data"]!["edit_history_tweet_ids"]![0]!.GetValue<string>();
    }

    private static long GetMediaId(string result)
    {
        JsonNode nodeResponse = JsonSerializer.Deserialize<JsonNode>(result)!;
        return nodeResponse["media_id"]!.GetValue<long>();
    }
}