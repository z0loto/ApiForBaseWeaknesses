using ApiForBaseWeaknesses.Dtos.ScanDto.ScanRequestDto;
using ApiForBaseWeaknesses.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class HostsController(AppDbContext context, ILogger<HostsController> logger, ConvertToEntityService convert,
    IMapper mapper) : ControllerBase
{
    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            logger.LogDebug("Файл отсутствует или пуст");
            return BadRequest("Файл отсутствует или пуст");
        }

        try
        {
            var entityHosts = await convert.ConvertToEntity(file);
            await context.Hosts.AddRangeAsync(entityHosts);
            await context.SaveChangesAsync();
            return Ok($"Добавлено хостов: {entityHosts.Count}");
        }
        catch (Exception ex)
        {
            logger.LogDebug($"Непредвиденная ошибка: {ex.Message}");
            return BadRequest("Непредвиденная ошибка");
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var hosts = await context.Hosts.ProjectTo<HostRequestDto>(mapper.ConfigurationProvider).ToListAsync();
        if (hosts.Count == 0)
        {
            return Ok(new List<HostRequestDto>());
        }

        return Ok(hosts);
    }
}