using System.Text;
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
        
        string[] anecdotes = SplitByMaxSize(candidat.anecdotes);
        parts = await BuildAnecdotes(gptOptions, parts, anecdotes);
        
        parts = parts.Concat(candidat.refs).ToList();

        return parts;
    }

    private static string[] SplitByMaxSize(string[] candidatAnecdotes)
    {
        const int MAX_SIZE = 280;
        string[] result = [];

        foreach (string anecdote in candidatAnecdotes)
        {
            string[] substrings = [];

            string[] words = anecdote.Split();
            StringBuilder currentString = new();

            foreach (string word in words)
            {
                if (currentString.Length + word.Length + 1 > MAX_SIZE)
                {
                    substrings = substrings.Append(currentString.ToString().Trim()).ToArray();
                    currentString.Clear();
                }
                currentString.Append(word + " ");
            }

            if (currentString.Length > 0)
            {
                substrings = substrings.Append(currentString.ToString().Trim()).ToArray();
            }

            result = result.Concat(substrings).ToArray();
        }
        
        return result;
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
}