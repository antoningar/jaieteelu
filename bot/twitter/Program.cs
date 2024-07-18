using Microsoft.Extensions.Configuration;
using twitter.Deputy;
using twitter.X;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

XOptions xOptions = new();
configuration.GetSection("Twitter").Bind(xOptions);

try {
    Deputy deputy = DeputyService.GetDeputies();
    Console.WriteLine($"Deputy : {deputy.Name}");

    await XService.PostDeputy(xOptions, deputy.SplitDeputy());

    DeputyService.SaveDeputy(deputy.Name);
}
catch (Exception ex)
{
    Console.WriteLine($"Error {ex.Message}");
    Environment.Exit(-1);
}