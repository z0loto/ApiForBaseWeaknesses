using ApiForBaseWeaknesses.Dtos.ScanDto.ScanRequestDto;
using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class ScansController(AppDbContext context, IMapper mapper, GeneratorVulnerabilitiesService generator)
    : ControllerBase
{
    [HttpGet("date-range")]
    public async Task<ActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        int page)
    {
        if (startDate > endDate)
        {
            return BadRequest();
        }

        const int pageSize = 2;
        var validScans = context.Scans.Where(s => s.ScannedAt >= startDate &&
                                                  s.ScannedAt <= endDate)
            .OrderBy(s => s.ScannedAt);
        var scans = validScans.Skip((page - 1)).Take(pageSize);
        var scanDtos = await scans.ProjectTo<ScanResposnseDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(scanDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOne(int id)
    {
        var scan = await context.Scans.FirstOrDefaultAsync(s => s.Id == id);
        if (scan == null)
        {
            return NotFound();
        }

        var scanResponseDto = await context.Scans
            .Where(s => s.Id == id)
            .ProjectTo<ScanResposnseDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return Ok(scanResponseDto);
    }

    [HttpPost("host-scanning")]
    public async Task<ActionResult> Scans(List<HostRequestDto> hostIndexes)
    {
        var hosts = await context.Hosts.Where(h => hostIndexes.Select(hI => hI.Id)
            .Contains(h.Id)).ToListAsync();
        if (hosts.Count == 0)
        {
            return BadRequest();
        }

        var maxVulnerabilities = await context.Vulnerabilities.CountAsync();
        if (maxVulnerabilities == 0)
        {
            return Ok("Угроз не обнаружено");
        }

        var hostVulnerabilities = new Dictionary<int, int>();

        foreach (var host in hosts)
        {
            var scan = await generator.Generate(host, maxVulnerabilities);
            await context.Scans.AddAsync(scan);
            await context.SaveChangesAsync();
            hostVulnerabilities.Add(host.Id, scan.ScanVulnerability.Select(sv => sv.Vulnerability).ToList().Count);
        }

        var report = "Обнаружено уязвимостей:\n" + string.Join("\n", hostVulnerabilities
            .Select(hv => $"Хост {hv.Key} — {hv.Value} уязвимостей"));
        return Ok(report);
    }
}