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
                $"voici un tableau d'anecdotes:\n{string.Join("\n", anecddotes)}\nreformule chacune de ces anecdotes a la 1ere personne du singulier, ne numérotes les numérote pas mais sépare les par \" :: \"")
        ];
    }
}