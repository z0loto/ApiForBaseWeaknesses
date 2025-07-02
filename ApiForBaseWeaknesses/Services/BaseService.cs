using System.Text.Json;
using ApiForBaseWeaknesses.Dto;

namespace ApiForBaseWeaknesses.Services;

public class BaseService
{
    public bool FiilBase()
    {
        string json = File.ReadAllText("import/nvdcve-2.0-recent.json");
        // Настройка опций для camelCase, для решения проблемы заглавных букв
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        MainDto response = JsonSerializer.Deserialize<MainDto>(json, options);
        Console.WriteLine("DTO загружен: " + (response != null));
        Console.WriteLine(response.Vulnerabilities.Count);
        File.WriteAllText("output.txt", JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));


        return true;
    }
}