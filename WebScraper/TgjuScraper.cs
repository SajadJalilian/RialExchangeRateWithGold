using System.Net.Http.Headers;
using System.Text.Json;

namespace WebScraper;

public class TgjuScraper
{
    public async Task ReadData()
    {
        await using var appDbContext = new AppDbContext();

        var request = RequestBuilder.Build(0);
        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return;

        var result = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

        for (var i = 0; i < apiResponse.RecordsTotal; i += 30)
        {
            var rb = RequestBuilder.Build(i);
            var r = await client.SendAsync(rb);
            if (!r.IsSuccessStatusCode) return;

            var res = await r.Content.ReadAsStringAsync();
            var apires = JsonSerializer.Deserialize<ApiResponse>(res);

            var goldData = DataExtractor.Extract(apires.Data);

            await Task.Delay(TimeSpan.FromSeconds(3));

            appDbContext.GoldDatas.AddRange(goldData);
            await appDbContext.SaveChangesAsync();
        }
    }
}