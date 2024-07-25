using Microsoft.Extensions.Configuration;
using twitter.Deputy;
using twitter.X;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

XOptions xOptions = new();
configuration.GetSection("Twitter").Bind(xOptions);

string picturesPath = configuration.GetSection("PicturesPath").Value!;

try {
    Deputy deputy = DeputyService.GetDeputies();
    Console.WriteLine($"Deputy : {deputy.Name}");

    FileInfo fileInfo = new(Path.Combine(picturesPath, deputy.Name.Replace(" ", "_") + ".jpg"));
    if (!fileInfo.Exists)
    {
        Environment.Exit(-1);    
    }
    
    long mediaId = await XService.UploadPicturesAsync(xOptions, fileInfo.FullName);
    await XService.PostDeputy(xOptions, deputy.SplitDeputy(), mediaId);

    DeputyService.SaveDeputy(deputy.Name);
}
catch (Exception ex)
{
    Console.WriteLine($"Error {ex.Message}");
    Environment.Exit(-1);
}