using System.Text;
using twitter.Gpt;

namespace twitter.Deputy;

public static class JsonService
{
    public static async Task<IEnumerable<string>> Build(Deputy deputy, GptOptions gptOptions)
    {
        List<string> parts =
        [
            GetFirstPart(deputy.name, deputy.constituency),
        ];
        
        string[] facts = SplitByMaxSize(deputy.facts);
        parts = await BuildFacts(gptOptions, parts, facts);
        
        parts = parts.Concat(deputy.refs).ToList();

        return parts;
    }

    private static string[] SplitByMaxSize(string[] deputyFacts)
    {
        const int MAX_SIZE = 280;
        string[] result = [];

        foreach (string fact in deputyFacts)
        {
            string[] substrings = [];

            string[] words = fact.Split();
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

    private static string GetFirstPart(string name, string constituency)
    {
        return $"Je suis {name}, {constituency}";
    }

    private static async Task<List<string>> BuildFacts(GptOptions gptOption, List<string> parts, string[] facts)
    {
        string[] newFacts = await GptService.BuildAnecdotesAsync(gptOption, facts);
        parts.AddRange(newFacts);
        return parts;
    }
}