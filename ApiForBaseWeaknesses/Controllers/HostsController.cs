using ApiForBaseWeaknesses.Requests;
using ApiForBaseWeaknesses.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly ConvertToEntityService _convert;
    private readonly IMapper _mapper;
    public HostsController(AppDbContext context, ILogger<HostsController> logger, ConvertToEntityService convert,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _convert = convert;
        _mapper = mapper;
        
    }
    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogDebug("Файл отсутствует или пуст");
            return BadRequest("Файл отсутствует или пуст");
        }

        try
        {
            using var stream = file.OpenReadStream();
            var entityHosts = await _convert.ConvertToHost(stream);
            await _context.Hosts.AddRangeAsync(entityHosts);
            await _context.SaveChangesAsync();
            return Ok($"Добавлено хостов: {entityHosts.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Непредвиденная ошибка: {ex.Message}");
            return BadRequest("Непредвиденная ошибка");
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var hosts = await _context.Hosts.ProjectTo<HostIndexes>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(hosts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var host = await _context.Hosts.FindAsync(id);

        if (host == null)
            return NotFound();
        var hostRequest = _mapper.Map<Dtos.Host>(host);
        return Ok(hostRequest);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Dtos.Host newHost)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var hostEntity = _mapper.Map<Host>(newHost);
        hostEntity.CreatedAt = DateTime.UtcNow;
        _context.Hosts.Add(hostEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = hostEntity.Id }, newHost);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] Dtos.Host updatedHost)
    {
        var host = await _context.Hosts.FindAsync(updatedHost.Ip);
        if (host == null)
            return NotFound();

        host.Ip = updatedHost.Ip;
        host.Description = updatedHost.Description;

        await _context.SaveChangesAsync();

        return Ok(host);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var host = await _context.Hosts.FindAsync(id);
        if (host == null)
            return NotFound();

        _context.Hosts.Remove(host);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}