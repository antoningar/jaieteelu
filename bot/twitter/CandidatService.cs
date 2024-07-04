using System.Text.Json;

namespace twitter;

public static class CandidatService
{
    private const string BASE_CANDIDATS_FILENAME = "candidats.json";
    private const string OLD_FILENAME = "old_candidats.json";

    public static Candidat GetCandidats()
    {
        IEnumerable<Candidat> baseCandidats = GetBaseCandidats();
        List<string> oldCandidats = GetOldCandidats().ToList();

        Candidat candidat = GetCandidat(baseCandidats.ToArray(), oldCandidats);

        return candidat;
    }

    public static void SaveCandidat(string candidatNom)
    {
        List<string> oldCandidats = GetOldCandidats().ToList();
        oldCandidats.Add(candidatNom);
        string jsonString = JsonSerializer.Serialize(oldCandidats);
        File.WriteAllText(OLD_FILENAME, jsonString);
    }

    private static IEnumerable<Candidat> GetBaseCandidats()
    {
        string content = File.ReadAllText(BASE_CANDIDATS_FILENAME);
        return JsonSerializer.Deserialize<IEnumerable<Candidat>>(content)!;
    }

    private static IEnumerable<string> GetOldCandidats()
    {
        string content = File.ReadAllText(OLD_FILENAME);
        return JsonSerializer.Deserialize<IEnumerable<string>>(content)!;
    }

    private static Candidat GetCandidat(Candidat[] baseCandidats, List<string> oldCandidats)
    {
        Candidat? choosenCandidat;
    
        do
        {
            Candidat tmpCandidat = GetRandomCandidat(baseCandidats.ToArray());
            choosenCandidat = oldCandidats.Contains(tmpCandidat.nom) ? null : tmpCandidat;
        } while (choosenCandidat is null);

        return choosenCandidat;
    }

    private static Candidat GetRandomCandidat(IEnumerable<Candidat> candidats)
    {
        Random random = new();
        List<Candidat> candidateList = candidats.ToList();
        if (candidateList.Count == 0)
            throw new InvalidOperationException("No candidates available");

        int index = random.Next(candidateList.Count);
        return candidateList[index];
    }
}
