using System.Text.Json;
using ApiForBaseWeaknesses.Dto;
using Microsoft.AspNetCore.Mvc;
using ApiForBaseWeaknesses.Models;

namespace ApiForBaseWeaknesses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VulnerabilitiesController(
        ILogger<VulnerabilitiesController> logger,
        AppDbContext context)
        : ControllerBase
    {
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile? file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    logger.LogDebug("Файл отсутствует или пуст");
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
                MainVulnerabilitiesDto? mainVulnerabilitiesDto =
                    JsonSerializer.Deserialize<MainVulnerabilitiesDto>(json, options);
                if (mainVulnerabilitiesDto != null)
                {
                    List<Vulnerability> vulnerabilities = Mapping.ImportMapper
                        .MapToListVulnerability(mainVulnerabilitiesDto);
                    await context.Vulnerabilities.AddRangeAsync(vulnerabilities);
                    await context.SaveChangesAsync();
                    return Ok($"Добавлено уязвимостей: {vulnerabilities.Count}");
                }

                return Ok("Новых уязвимостей не добавлено");
            }
            catch (Exception ex)
            {
                logger.LogDebug("Непредвиденная ошибка");
                return BadRequest("Непредвиденная ошибка");
            }
        }
    }
}