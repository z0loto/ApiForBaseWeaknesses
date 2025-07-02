using System.Text.Json;
using ApiForBaseWeaknesses.Dto;
using ApiForBaseWeaknesses.Models;
using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Services;

public class BaseService
{
    private readonly AppDbContext _context;

    public BaseService(AppDbContext context)
    {
        _context = context;
    }
    public bool FiilBase()
    {
        string json = File.ReadAllText("import/nvdcve-2.0-recent.json");
        var options = new JsonSerializerOptions
        // Настройка опций для camelCase, для решения проблемы заглавных букв
        {
            PropertyNameCaseInsensitive = true
        };
        MainDto response = JsonSerializer.Deserialize<MainDto>(json, options);
        File.WriteAllText("outputDto.txt", JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
        FillModels(response);
        return true;
    }

    public void FillModels(MainDto response)
    {
        if (response != null)
        {
            List<Vulnerability> Finalmodel = Mapping.Mapping.MapToListVulnerability(response);
            // Настройка сериализации для понятного вывода
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, 
                ReferenceHandler = ReferenceHandler.IgnoreCycles 
            };
            File.WriteAllText("OutputFinalModels.txt", JsonSerializer.Serialize(Finalmodel, jsonOptions));
            Console.WriteLine("OutputFinal выведен. Переход к FinalFill");
            FinalFill(Finalmodel);
            
        }
    }

    public void FinalFill(List<Vulnerability> Finalmodel)
    {
        Console.WriteLine("FinalFill сработала");
        _context.Vulnerability.AddRange(Finalmodel);
        _context.SaveChanges();
        Console.WriteLine("Успех?");
    }
}