using WebScraper;

Console.WriteLine("App is starting");
Console.WriteLine("Pick a delay time between api calls in seconds.");
bool validNumber = false;
int delayTimeInSeconds = 0;
while (!validNumber)
{
    var inSeconds = Console.ReadLine();
    validNumber = int.TryParse(inSeconds, out int num);
    if (validNumber)
        delayTimeInSeconds += num;
    else
        Console.WriteLine("Pick a valid number in seconds");
}

if (delayTimeInSeconds is 0)
    delayTimeInSeconds = 1;

var scraper = new TgjuScraper();
await scraper.ReadData(delayTimeInSeconds);

Console.WriteLine("App is finished");