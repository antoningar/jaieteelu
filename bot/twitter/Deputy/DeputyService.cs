using System.Text.Json;

namespace twitter.Deputy;

public static class DeputyService
{
    private const string BASE_DEPUTY_FILENAME = "deputies.json";
    private const string OLD_DEPUTY_FILENAME = "old_deputies.json";

    public static Deputy GetDeputies()
    {
        IEnumerable<Deputy> baseDeputies = GetBaseDeputies();
        List<string> oldDeputies = GetOldDeputies().ToList();

        Deputy deputy = GetDeputy(baseDeputies.ToArray(), oldDeputies);

        return deputy;
    }

    public static void SaveDeputy(string deputyName)
    {
        List<string> oldDeputies = GetOldDeputies().ToList();
        oldDeputies.Add(deputyName);
        string jsonString = JsonSerializer.Serialize(oldDeputies);
        File.WriteAllText(OLD_DEPUTY_FILENAME, jsonString);
    }

    private static IEnumerable<Deputy> GetBaseDeputies()
    {
        string content = File.ReadAllText(BASE_DEPUTY_FILENAME);
        return JsonSerializer.Deserialize<IEnumerable<Deputy>>(content)!;
    }

    private static IEnumerable<string> GetOldDeputies()
    {
        string content = File.ReadAllText(OLD_DEPUTY_FILENAME);
        return JsonSerializer.Deserialize<IEnumerable<string>>(content)!;
    }

    private static Deputy GetDeputy(Deputy[] baseDeputies, List<string> oldDeputies)
    {
        Deputy? choosenDeputy;
    
        do
        {
            Deputy tmpDeputy = GetRandomDeputy(baseDeputies.ToArray());
            choosenDeputy = oldDeputies.Contains(tmpDeputy.name) ? null : tmpDeputy;
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
