using WebScraper;

Console.WriteLine("App is starting");

var scraper = new TgjuScraper();
await scraper.ReadData();

Console.WriteLine("App is finished");
