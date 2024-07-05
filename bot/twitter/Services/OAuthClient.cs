using System.Text;
using Microsoft.Extensions.Options;
using OAuth;
using twitter.Options;

namespace twitter.Services;

public class OAuthClient
{
    public static async Task<string> MakeAuthenticatedRequestAsync(IOptions<TwitterOption> options, string resourceUrl, HttpMethod method, string body)
    {
        OAuthRequest oauth = new()
        {
            Method = method.ToString(),
            Type = OAuthRequestType.ProtectedResource,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = options.Value.ConsumerKey ,
            ConsumerSecret = options.Value.ConsumerSecret,
            Token = options.Value.AccessToken,
            TokenSecret = options.Value.AccessTokenSecret,
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