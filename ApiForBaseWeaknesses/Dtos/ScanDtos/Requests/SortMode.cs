using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dtos.ScanDtos.Requests;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortMode
{
    Id,
    Date
}