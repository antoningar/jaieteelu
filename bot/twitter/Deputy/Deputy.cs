using System.Text;
using System.Text.Json.Serialization;

namespace twitter.Deputy;

public class Deputy(string name, string constituency, string[] facts, string[] refs)
{
    [JsonPropertyName("nom")]
    public string Name { get; set; } = name;
    [JsonPropertyName("circonscription")]
    public string Constituency { get; set; } = constituency;
    [JsonPropertyName("anecdotes")]
    public string[] Facts { get; set; } = facts;
    [JsonPropertyName("refs")]
    public string[] Refs { get; set; } = refs;

    public List<string> SplitDeputy()
    {
        List<string> parts =
        [
            $"Je suis {Name}, {Constituency}"
        ];
        
        return parts
            .Concat(SplitFacts())
            .Concat(Refs)
            .ToList();
    }

    private string[] SplitFacts()
    {
        const int MAX_SIZE = 280;
        string[] result = [];

        foreach (string fact in Facts)
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
}