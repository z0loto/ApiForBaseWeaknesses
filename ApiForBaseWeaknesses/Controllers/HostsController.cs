using ApiForBaseWeaknesses.Models;
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

            List<Host> result = Mapping.Mapping.MapToHost(hosts);
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

    [HttpGet("scan/{id}")]
    public async Task<ActionResult> Scan(int id)
    {
        var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);
        if (host != null)
        {
            var useIndexes = new HashSet<int>();
            int maxValue = await _context.Vulnerabilities.CountAsync();
            if (maxValue == 0)
            {
                return BadRequest();
            }

            int minIndex = await _context.Vulnerabilities.MinAsync(v => v.Id);
            int maxIndex = await _context.Vulnerabilities.MaxAsync(v => v.Id);
            Random random = new Random();
            int numberVulnerabilities = random.Next(maxValue);
            while (useIndexes.Count < numberVulnerabilities)
            {
                useIndexes.Add(random.Next(minIndex, maxIndex));
            }

            var validIndexes = await _context.Vulnerabilities
                .Where(v => useIndexes.Contains(v.Id)).Select(v => v.Id).ToListAsync();
            var scan = new Scan
            {
                ScannedAt = DateTime.UtcNow.Date,
                HostId = host.Id
            };
            await _context.Scans.AddAsync(scan);
            await _context.SaveChangesAsync();
            var scanVulnerability = validIndexes.Select(id => new ScanVulnerability
            {
                ScanId = scan.Id,
                VulnerabilityId = id
            }).ToList();
            await _context.ScanVulnerabilities.AddRangeAsync(scanVulnerability);
            await _context.SaveChangesAsync();
            return Ok($"Обнаружено уязвимостей: {scanVulnerability.Count()}");
        }

        return NotFound();
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        var hosts = await _context.Hosts.Select(h => new
        {
            h.Id,
            h.Ip
        }).ToListAsync();
        if (hosts.Count == 0)
        {
            return NotFound();
        }

        return Ok(hosts);
    }
}