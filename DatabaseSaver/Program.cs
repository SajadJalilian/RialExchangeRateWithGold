using WebScraper;

using var appDbContext = new AppDbContext();

var tgjuScraper = new TgjuScraper();
var data = await tgjuScraper.ReadData(3);

Console.WriteLine("Saving to DB");

appDbContext.GoldDatas.AddRange(data.Select(x => new GoldDataEntity
{
    RialPrice = x.RialPrice,
    Date = x.Date
}));

await appDbContext.SaveChangesAsync();

Console.WriteLine("Finished");