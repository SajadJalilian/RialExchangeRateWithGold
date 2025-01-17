using System.Text.Json;

namespace WebScraper;

public class TgjuScraper
{
    public async Task<GoldDataModel[]> ReadData(int delay)
    {
        var totalGoldData = new List<GoldDataModel>();

        using var client = new HttpClient();
        var (_, recordsTotal) = await ReadLatestPage();

        Console.WriteLine($"Start scrapping data. Total record is {recordsTotal}");
        var estimatedTime = recordsTotal / 30 * delay / 60;
        Console.WriteLine($"Estimated time is {estimatedTime} Minutes with {delay} seconds delay between each call");

        for (var i = 0; i < recordsTotal; i += 30)
        {
            var rb = RequestBuilder.Build(i);
            var r = await client.SendAsync(rb);
            if (!r.IsSuccessStatusCode) continue;

            var res = await r.Content.ReadAsStringAsync();
            var apires = JsonSerializer.Deserialize<ApiResponse>(res);

            var goldData = DataExtractor.Extract(apires.Data);
            totalGoldData.AddRange(goldData);

            Console.WriteLine($"{totalGoldData.Count} records scrapped");

            await Task.Delay(TimeSpan.FromSeconds(3));
        }

        return totalGoldData.ToArray();
    }

    public async Task<(GoldDataModel[] data, int recordsToal)> ReadLatestPage()
    {
        var request = RequestBuilder.Build(0);
        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return ([], 0);

        var result = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);
        var goldData = DataExtractor.Extract(apiResponse.Data);

        return (goldData, apiResponse.RecordsTotal);
    }
}