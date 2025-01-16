namespace WebScraper;

public static class DataExtractor
{
    public static GoldDataModel[] Extract(List<List<string>> apiResponseData)
    {
        var dataList = new List<GoldDataModel>();
        foreach (var column in apiResponseData)
        {
            var price = decimal.Parse(column[3]);
            var date = DateOnly.Parse(column[6]);

            var goldData = new GoldDataModel
            {
                RialPrice = price,
                Date = date
            };
            dataList.Add(goldData);
        }

        return dataList.ToArray();
    }
}