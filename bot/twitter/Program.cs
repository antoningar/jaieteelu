using Microsoft.Extensions.Configuration;
using twitter.Deputy;
using twitter.Gpt;
using twitter.X;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

XOptions xOptions = new();
configuration.GetSection("Twitter").Bind(xOptions);

GptOptions gptOption = new();
configuration.GetSection("Gpt").Bind(gptOption);

try {
    Deputy deputy = DeputyService.GetDeputies();
    Console.WriteLine($"Deputy : {deputy.name}");
    IEnumerable<string> parts = await JsonService.Build(deputy, gptOption);

    await XService.PostDeputy(xOptions, parts);

    DeputyService.SaveDeputy(deputy.name);
}
catch (Exception ex)
{
    Console.WriteLine($"Error {ex.Message}");
    Environment.Exit(-1);
}