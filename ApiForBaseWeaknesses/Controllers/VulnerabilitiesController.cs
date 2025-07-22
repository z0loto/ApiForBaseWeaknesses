using System.Text.Json;
using ApiForBaseWeaknesses.Dtos.ImportDtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiForBaseWeaknesses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VulnerabilitiesController
        : ControllerBase
    {
        private readonly ILogger<VulnerabilitiesController> _logger;
        private readonly AppDbContext _context;
        public VulnerabilitiesController(ILogger<VulnerabilitiesController> logger, AppDbContext context)
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
                var mainVulnerabilitiesDto =
                    JsonSerializer.Deserialize<MainVulnerabilities>(json, options);
                if (mainVulnerabilitiesDto == null)
                {
                    return Ok("Новых уязвимостей не добавлено");
                }

                var vulnerabilities = Mapping.ImportMapper
                    .MapToListVulnerability(mainVulnerabilitiesDto);
                await _context.Vulnerabilities.AddRangeAsync(vulnerabilities);
                await _context.SaveChangesAsync();
                return Ok($"Добавлено уязвимостей: {vulnerabilities.Count}");
                
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Непредвиденная ошибка");
                return BadRequest("Непредвиденная ошибка");
            }
        }
    }
}