using Microsoft.Extensions.Configuration;
using twitter.Deputy;
using twitter.X;
using FileOptions = twitter.Deputy.FileOptions;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

XOptions xOptions = new();
configuration.GetSection("Twitter").Bind(xOptions);

FileOptions fileOptions = new();
configuration.GetSectio>n("FilesPaths").Bind(fileOptions);

try {
    Deputy deputy = DeputyService.GetDeputies(fileOptions.BaseDeputyFilePath, fileOptions.OldDeputyFilePath);
    Console.WriteLine($"Deputy : {deputy.Name}");

    FileInfo fileInfo = new(Path.Combine(fileOptions.PictureFilePath, deputy.Name.Replace(" ", "_") + ".jpg"));
    if (!fileInfo.Exists)
    {
        Console.WriteLine($"File {fileInfo.Name} not found");
        Environment.Exit(-1);    
    }
    
    long mediaId = await XService.UploadPicturesAsync(xOptions, fileInfo.FullName);
    
    await XService.PostDeputy(xOptions, deputy.SplitDeputy(), mediaId);
    DeputyService.SaveDeputy(deputy.Name, fileOptions.OldDeputyFilePath);
}
catch (Exception ex)
{
    Console.WriteLine($"Error {ex.Message}");
    Environment.Exit(-1);
}