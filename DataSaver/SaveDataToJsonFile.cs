using System.Text.Json;
using WebScraper;

namespace DatabaseSaver;

public static class SaveDataToJsonFile
{
    private const string ArtifactDir = "artifact";
    private const string Gold24CaratPrice = "Gold24Carat";
    private const string FileName = $"{Gold24CaratPrice}.json";
    private const string FileNameMin = $"{Gold24CaratPrice}_min.json";

    public static bool Save()
    {
        string baseDirectory = AppContext.BaseDirectory;
        var path = $"{baseDirectory}/{ArtifactDir}";
        
        using var appDbContext = new AppDbContext();

        var data = appDbContext.GoldDatas.ToArray();

        var trimmedData = data.Select(x => new GoldDataModel
        {
            RialPrice = x.RialPrice,
            Date = x.Date
        }).ToArray();

        var options = new JsonSerializerOptions { WriteIndented = true };
        var formattedJsonString = JsonSerializer.Serialize(trimmedData, options);
        var minifiedJsonString = JsonSerializer.Serialize(trimmedData);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        var pathNormal = Path.Combine(path,FileName);
        File.WriteAllTextAsync(pathNormal, formattedJsonString);
        
        var pathMin = Path.Combine(path,FileNameMin);
        File.WriteAllTextAsync(pathMin, minifiedJsonString);

        Console.WriteLine($"File saved to {ArtifactDir}");

        return true;
    }
}