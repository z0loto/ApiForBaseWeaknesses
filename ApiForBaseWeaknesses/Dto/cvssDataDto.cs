namespace ApiForBaseWeaknesses.Dto;

public class cvssDataDto
{
    public string Version { get; set; } = string.Empty;
    public string VectorString { get; set; } = string.Empty;
    //обновить запрос на парсинг с атрибутом BaseScope
    public double BaseScore { get; set; } = new();
}