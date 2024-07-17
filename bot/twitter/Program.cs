using Microsoft.Extensions.Configuration;
using twitter;
using twitter.Candidats;
using twitter.Gpt;
using twitter.X;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

XOptions xOptions = new();
configuration.GetSection("Twitter").Bind(xOptions);

GptOptions gptOption = new();
configuration.GetSection("Gpt").Bind(gptOption);

Candidat candidat = CandidatService.GetCandidats();
Console.WriteLine($"Candidat : {candidat.nom}");
IEnumerable<string> parts = await JsonService.Build(candidat, gptOption);

await XService.PostCandidat(xOptions, parts);

CandidatService.SaveCandidat(candidat.nom);