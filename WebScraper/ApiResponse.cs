using System.Text.Json.Serialization;

namespace WebScraper;

public record ApiResponse(
    [property: JsonPropertyName("draw")]
    string Draw,
    [property: JsonPropertyName("recordsTotal")]
    int RecordsTotal,
    [property: JsonPropertyName("data")]
    List<List<string>> Data
);