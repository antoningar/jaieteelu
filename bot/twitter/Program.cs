using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using twitter;
using twitter.Candidats;
using twitter.Gpt;
using twitter.Options;
using twitter.Services;

    
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

TwitterOption twitterOption = new();
configuration.GetSection("Twitter").Bind(twitterOption);

GptOptions gptOption = new();
configuration.GetSection("Gpt").Bind(gptOption);

Candidat candidat = CandidatService.GetCandidats();

IEnumerable<string> parts = await BuilderService.Build(candidat, gptOption);
// SendTweetAsync(Options.Create(), parts);
IEnumerable<string> parts = await JsonService.Build(candidat, gptOption);

CandidatService.SaveCandidat(candidat.nom);

async Task SendTweetAsync(IOptions<TwitterOption> options, IEnumerable<string> tweets){
    string url = "https://api.twitter.com/2/tweets";
    string simpleTweet = "{\"text\": \"Tiens tiens tiens\"}";
    string replyTweet = "{\"text\": \"Tiens tiens retiens\", \"reply\": {\"in_reply_to_tweet_id\": \"1808624245003452927\"}}";

    string result = await OAuthClient.MakeAuthenticatedRequestAsync(options, url, HttpMethod.Post, replyTweet);
    Console.WriteLine(result);
}