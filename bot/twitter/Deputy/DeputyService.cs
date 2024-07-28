using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace twitter.Deputy;

public static class DeputyService
{

    public static Deputy GetDeputies(string baseDeputyFilename, string oldDeputyFilename)
    {
        IEnumerable<Deputy> baseDeputies = GetBaseDeputies(baseDeputyFilename);
        List<string> oldDeputies = GetOldDeputies(oldDeputyFilename).ToList();

        Deputy deputy = GetDeputy(baseDeputies.ToArray(), oldDeputies);

        return deputy;
    }

    public static void SaveDeputy(string deputyName, string filename)
    {
        List<string> oldDeputies = GetOldDeputies(filename).ToList();
        oldDeputies.Add(deputyName);
        
        JsonSerializerOptions jsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        string jsonString = JsonSerializer.Serialize(oldDeputies, jsonOptions);
        
        File.WriteAllText(filename, jsonString, Encoding.UTF8);
    }

    private static IEnumerable<Deputy> GetBaseDeputies(string filename)
    {
        string content = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<IEnumerable<Deputy>>(content)!;
    }

    private static IEnumerable<string> GetOldDeputies(string filename)
    {
        string content = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<IEnumerable<string>>(content)!;
    }

    private static Deputy GetDeputy(Deputy[] baseDeputies, List<string> oldDeputies)
    {
        Deputy? choosenDeputy;
    
        do
        {
            Deputy tmpDeputy = GetRandomDeputy(baseDeputies.ToArray());
            choosenDeputy = oldDeputies.Contains(tmpDeputy.Name) ? null : tmpDeputy;
        } while (choosenDeputy is null);

        return choosenDeputy;
    }

    private static Deputy GetRandomDeputy(IEnumerable<Deputy> deputies)
    {
        Random random = new();
        List<Deputy> deputiesList = deputies.ToList();
        if (deputiesList.Count == 0)
            throw new InvalidOperationException("No deputies available");

        int index = random.Next(deputiesList.Count);
        return deputiesList[index];
    }
}
