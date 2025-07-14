using System.Text.Json;
using ApiForBaseWeaknesses.Dto;
using Microsoft.AspNetCore.Mvc;
using ApiForBaseWeaknesses.Models;

namespace ApiForBaseWeaknesses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VulnerabilitiesController : ControllerBase
    {
        private readonly ILogger<VulnerabilitiesController> _logger;
        private readonly AppDbContext _context;

        public VulnerabilitiesController(ILogger<VulnerabilitiesController> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile? file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogDebug("Файл отсутствует или пуст");
                    return BadRequest("Файл отсутствует или пуст");
                }

                string json;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    json = await reader.ReadToEndAsync();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                MainVulnerabilitiesDto mainVulnerabilitiesDto =
                    JsonSerializer.Deserialize<MainVulnerabilitiesDto>(json, options);
                if (mainVulnerabilitiesDto != null)
                {
                    List<Vulnerability> finalmodel = Mapping.Mapping.MapToListVulnerability(mainVulnerabilitiesDto);
                    await _context.Vulnerabilities.AddRangeAsync(finalmodel);
                    await _context.SaveChangesAsync();
                    return Ok($"Добавлено уязвимостей: {finalmodel.Count()}");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Непредвиденная ошибка");
                return BadRequest("Непредвиденная ошибка");
            }
        }
    }
}