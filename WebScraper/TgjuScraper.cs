using System.Net.Http.Headers;
using System.Text.Json;

namespace WebScraper;

public class TgjuScraper
{
    public async Task ReadData()
    {
        await using var appDbContext = new AppDbContext();
        var urlQuery = QueryBuilder.Build(0, 30);

        using var httpClient = HttpClientBuilder();

        var responseMessage = await httpClient.GetAsync(urlQuery);
        if (!responseMessage.IsSuccessStatusCode) return;

        var result = await responseMessage.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

        var goldDatas = DataExtractor.Extract(apiResponse.Data);
        appDbContext.GoldDatas.AddRange(goldDatas);
        await appDbContext.SaveChangesAsync();

        for (var i = 30; i < apiResponse.RecordsTotal; i += 30)
        {
            var q = QueryBuilder.Build(i, 30);
            var rm = await httpClient.GetAsync(q);
            if (!rm.IsSuccessStatusCode) return;

            var goldData = DataExtractor.Extract(apiResponse.Data);
            Console.WriteLine(string.Join("--", goldData.Select(x => $"{x.Date}:::{x.RialPrice}")));

            await Task.Delay(TimeSpan.FromSeconds(3));

            appDbContext.GoldDatas.AddRange(goldData);
            await appDbContext.SaveChangesAsync();
        }
    }

    private static HttpClient HttpClientBuilder()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = false,
            MustRevalidate = true
        };
        return httpClient;
    }
}