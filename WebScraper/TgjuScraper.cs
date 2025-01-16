using System.Text.Json;

namespace WebScraper;

public class TgjuScraper
{
    public async Task<GoldDataModel[]> ReadData(int delay)
    {
        var totalGoldData = new List<GoldDataModel>();

        var request = RequestBuilder.Build(0);
        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return [];

        var result = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

        Console.WriteLine($"Start scrapping data. Total record is {apiResponse.RecordsTotal}");
        var estimatedTime = apiResponse.RecordsTotal / 30 * delay / 60;
        Console.WriteLine($"Estimated time is {estimatedTime} Minutes with {delay} seconds delay between each call");

        for (var i = 0; i < apiResponse.RecordsTotal; i += 30)
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
}