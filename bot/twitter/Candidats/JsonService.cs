using twitter.Gpt;

namespace twitter.Candidats;

public static class JsonService
{
    public static async Task<IEnumerable<string>> Build(Candidat candidat, GptOptions gptOptions)
    {
        List<string> parts =
        [
            GetFirstPart(candidat.nom, candidat.circonscription),
        ];
        parts = await BuildAnecdotes(gptOptions, parts, candidat.anecdotes);
        parts.Add(BuildRefs(candidat.refs));

        return parts;
    }

    private static string GetFirstPart(string nom, string circonscription)
    {
        return $"Je suis {nom}, {circonscription}";
    }

    private static async Task<List<string>> BuildAnecdotes(GptOptions gptOption, List<string> parts, string[] anecdotes)
    {
        string[] newAnecdotes = await GptService.BuildAnecdotesAsync(gptOption, anecdotes);
        parts.AddRange(newAnecdotes);
        return parts;
    }

    private static string BuildRefs(string[] refs)
    {
        return $"Source : {string.Join("\n", refs)}";
    }
}