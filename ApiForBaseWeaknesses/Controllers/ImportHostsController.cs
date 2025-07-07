using ApiForBaseWeaknesses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;
[ApiController]
[Route("[controller]")]

public class ImportHostsController:ControllerBase
{
  private readonly AppDbContext _context;
  private readonly ILogger<ImportHostsController> _logger;
  public ImportHostsController(AppDbContext context,ILogger<ImportHostsController> logger)
  {
    _context = context;
    _logger = logger;
  }
  [HttpPost("uploadHosts")]
  public async Task<ActionResult> UploadHosts(IFormFile file)
  {
    try
    {
      if (file.Length == 0)
      {
        _logger.LogError("Файл пуст");
        return BadRequest("Файл пуст");
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
      //List<NetworkHost> result = Mapping.Mapping.MapToNetworkHost(hosts);
      _logger.LogInformation("Пауза");
      //_context.Host.AddRange(result);
      _context.SaveChanges();
      return Ok(hosts);
    }
    catch (NullReferenceException ex)
    {
      _logger.LogError("Файл не передан: " + ex.Message);
      return NotFound("Файл не передан:");
    }
    catch (Exception ex)
    {
      _logger.LogError("Непредвиденная ошибка: " + ex.Message);
      return StatusCode(500, "Непредвиденная ошибка" );
    }
  }
}