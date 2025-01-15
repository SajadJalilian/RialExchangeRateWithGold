namespace WebScraper;

public static class DataExtractor
{
    public static GoldDataEntity[] Extract(List<List<string>> apiResponseData)
    {
        var dataList = new List<GoldDataEntity>();
        foreach (var column in apiResponseData)
        {
            var price = decimal.Parse(column[3]);
            var date = DateOnly.Parse(column[6]);

            var goldData = new GoldDataEntity
            {
                RialPrice = price,
                Date = date
            };
            dataList.Add(goldData);
        }

        return dataList.ToArray();
    }
}