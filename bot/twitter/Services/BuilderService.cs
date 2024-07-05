namespace twitter.Services;

public static class BuilderService
{
    public static IEnumerable<string> Build(Candidat candidat)
    {
        List<string> parts =
        [
            GetFirstPart(candidat.nom, candidat.circonscription),
        ];
        parts = BuildAnecdotes(parts, candidat.anecdotes);
        parts.Add(BuildRefs(candidat.refs));

        return parts;
    }

    private static string GetFirstPart(string nom, string circonscription)
    {
        return $"Je suis {nom}, {circonscription}";
    }

    private static List<string> BuildAnecdotes(List<string> parts, string[] anecdotes)
    {
        // parts.AddRange(GptService.GetPrompt(anecdotes));
        parts.AddRange(anecdotes);
        return parts;
    }

    private static string BuildRefs(string[] refs)
    {
        return $"Source : {string.Join("\n", refs)}";
    }
}