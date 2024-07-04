using twitter;

Candidat candidat = CandidatService.GetCandidats();
Console.WriteLine(candidat.nom);
CandidatService.SaveCandidat(candidat.nom);

async Task SendTweetAsync(IEnumerable<string> tweets){
    OAuthClient client = new();
    string url = "https://api.twitter.com/2/tweets";
    string simpleTweet = "{\"text\": \"Tiens tiens tiens\"}";
    string replyTweet = "{\"text\": \"Tiens tiens retiens\", \"reply\": {\"in_reply_to_tweet_id\": \"1808624245003452927\"}}";

    string result = await OAuthClient.MakeAuthenticatedRequestAsync(url, HttpMethod.Post, replyTweet);
    Console.WriteLine(result);
}