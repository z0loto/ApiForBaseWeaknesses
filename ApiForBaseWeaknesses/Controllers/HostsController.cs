using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class HostsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<HostsController> _logger;

    public HostsController(AppDbContext context, ILogger<HostsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile? file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogDebug("Файл отсутствует или пуст");
                return BadRequest("Файл отсутствует или пуст");
            }

            List<string> hosts = new();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        hosts.Add(line.Trim());
                    }
                }
            }

            List<Host> result = Mapping.ImportMapper.MapToHost(hosts);
            await _context.Hosts.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return Ok($"Добавлено хостов: {result.Count()}");
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Непредвиденная ошибка: " + ex.Message);
            return BadRequest("Непредвиденная ошибка");
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var hosts = await _context.Hosts.Select(h => new
        {
            h.Id,
            h.Ip,
            h.Description,
            h.CreatedAt
        }).ToListAsync();
        if (hosts.Count == 0)
        {
            return NotFound();
        }

        return Ok(hosts);
    }
}