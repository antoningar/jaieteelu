using System.Text;
using OAuth;

namespace twitter.Services;

public class OAuthClient
{
    private const string CONSUMER_KEY = "";
    private const string CONSUMER_SECRET = "";
    private const string ACCESS_TOKEN = "";
    private const string ACCESS_TOKEN_SECRET = "";

    public static async Task<string> MakeAuthenticatedRequestAsync(string resourceUrl, HttpMethod method, string body)
    {
        OAuthRequest oauth = new()
        {
            Method = method.ToString(),
            Type = OAuthRequestType.ProtectedResource,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = CONSUMER_KEY,
            ConsumerSecret = CONSUMER_SECRET,
            Token = ACCESS_TOKEN,
            TokenSecret = ACCESS_TOKEN_SECRET,
            RequestUrl = resourceUrl
        };

        string? authHeader = oauth.GetAuthorizationHeader();

        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

        HttpRequestMessage request = new (method, resourceUrl);
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}