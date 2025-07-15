using ApiForBaseWeaknesses.Dtos.ScanResultDto;
using ApiForBaseWeaknesses.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class ScansController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ScansController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //Сделать через пагинацию и увеличить кол-во передаваемых атрибутов
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var scans = await _context.Scans.Select(s => new
        {
            s.Id,
            s.HostId,
            s.ScannedAt,
            VulnerabilitiesCount = s.ScanVulnerability.Count
        }).ToListAsync();
        if (scans.Count == 0)
        {
            return Ok(new List<Scan>());
        }

        return Ok(scans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOne(int id)
    {
        var scan = await _context.Scans.FirstOrDefaultAsync(s => s.Id == id);
        if (scan == null)
        {
            return NotFound();
        }

        var scanResponseDto = await _context.Scans
            .Where(s => s.Id == id)
            .ProjectTo<ScanResposnsetDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return Ok(scanResponseDto);
    }

    [HttpPost("scans")]
    public async Task<ActionResult> Scans(List<int> hostIndexes)
    {
        var hosts = await _context.Hosts.Where(h => hostIndexes.Contains(h.Id)).ToListAsync();
        if (hosts.Count == 0)
        {
            return BadRequest();
        }

        var usedIndexes = new HashSet<int>();
        var maxValue = await _context.Vulnerabilities.CountAsync();
        if (maxValue == 0)
        {
            return Ok("Угроз не обнаружено");
        }

        var minIndex = await _context.Vulnerabilities.MinAsync(v => v.Id);
        var maxIndex = await _context.Vulnerabilities.MaxAsync(v => v.Id);
        var random = new Random();
        var hostVulnerabilities = new Dictionary<int, int>();

        foreach (var host in hosts)
        {
            var numberVulnerabilities = random.Next(maxValue);
            while (usedIndexes.Count < numberVulnerabilities)
            {
                usedIndexes.Add(random.Next(minIndex, maxIndex));
            }

            var validIndexes = await _context.Vulnerabilities
                .Where(v => usedIndexes.Contains(v.Id)).Select(v => v.Id).ToListAsync();
            var scan = new Scan
            {
                ScannedAt = DateTime.UtcNow.Date,
                HostId = host.Id
            };
            await _context.Scans.AddAsync(scan);
            await _context.SaveChangesAsync();

            var scanVulnerabilities = validIndexes.Select(index => new ScanVulnerability
            {
                ScanId = scan.Id,
                VulnerabilityId = index
            }).ToList();
            await _context.ScanVulnerabilities.AddRangeAsync(scanVulnerabilities);
            await _context.SaveChangesAsync();

            hostVulnerabilities.Add(host.Id, scanVulnerabilities.Count);
        }

        var report = "Обнаружено уязвимостей:\n" + string.Join("\n", hostVulnerabilities
            .Select(hv => $"Хост {hv.Key} — {hv.Value} уязвимостей"));
        return Ok(report);
    }
}