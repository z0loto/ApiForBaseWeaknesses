using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult> ImportHosts(IFormFile? file)
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

            List<Host> result = Mapping.Mapping.MapToNetworkHost(hosts);
            _context.Hosts.AddRange(result);
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Непредвиденная ошибка: " + ex.Message);
            return BadRequest("Непредвиденная ошибка");
        }
    }
}