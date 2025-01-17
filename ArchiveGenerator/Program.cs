using System.Text.Json;
using DatabaseSaver;
using WebScraper;

Console.WriteLine("Starting to generate archive for 24 carat gold rial per grams");

string baseDirectory = AppContext.BaseDirectory;

// Note: This tile will be added while github workflow running using git
var tempJsonPath = Path.Combine(baseDirectory, "temp_data.json");

string jsonContent = await File.ReadAllTextAsync(tempJsonPath);

var oldData = JsonSerializer.Deserialize<GoldDataModel[]>(jsonContent);
Console.WriteLine($"OldData count {oldData.Count()}");

var scraper = new TgjuScraper();
var (lastPageData, _) = await scraper.ReadLatestPage();

var lastDateInOldData = oldData?.OrderByDescending(x => x.Date).First().Date;
Console.WriteLine($"lastDateInOldData {lastDateInOldData}");

var newDataInLastPage = lastPageData.Where(x => x.Date > lastDateInOldData);
Console.WriteLine($"newDataInLastPage {newDataInLastPage.Count()}");

if (newDataInLastPage.Any())
{
    var dataToSaveInNewFile = new List<GoldDataModel>();
    dataToSaveInNewFile.AddRange(oldData);
    dataToSaveInNewFile.AddRange(newDataInLastPage);
    var orderByDescending = dataToSaveInNewFile.OrderByDescending(x => x.Date);
    var res = SaveDataToJsonFile.Save(orderByDescending.ToArray());
    Console.WriteLine(res);
}
else
{
    Console.WriteLine("false");
}