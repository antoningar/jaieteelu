using System.Text.Json.Serialization;

namespace twitter.Gpt;

public class GptRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("messages")]
    public GptMessage[] Messages { get; set; }

    public GptRequest(string model, List<string> anecddotes)
    {
        Model = model;
        Messages =
        [
            new GptMessage(
                $"voici un tableau json de string:\n{string.Join("\n", anecddotes)}\nce sont toutes des phrase qui decrivent ce qu'a deja fait une personne\nréecrit ces textes de maniere a ce quil soit ecrit a la 1ere personne du singulier, ne numérotes pas les anecdotes en debut de phrase et sépare chaque phrase par \"::\"")
        ];
    }
}