using DatabaseSaver;

Console.WriteLine("Starting to generate archive for 24 carat gold rial per grams");
var res = SaveDataToJsonFile.Save();

Console.WriteLine(res);