using ApiForBaseWeaknesses.Dtos.ScanDto.ScanRequestDto;
using ApiForBaseWeaknesses.Dtos.ScanResultDto;
using ApiForBaseWeaknesses.Models;
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
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var scans = await context.Scans.Select(s => new
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
        var scan = await context.Scans.FirstOrDefaultAsync(s => s.Id == id);
        if (scan == null)
        {
            return NotFound();
        }

        var scanResponseDto = await context.Scans
            .Where(s => s.Id == id)
            .ProjectTo<ScanResposnsetDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return Ok(scanResponseDto);
    }

    [HttpPost("start-scan")]
    public async Task<ActionResult> Scans(List<HostRequestDto> hostIndexes)
    {
        var hosts = await context.Hosts.Where(h => hostIndexes.Select(hI => hI.Id).Contains(h.Id)).ToListAsync();
        if (hosts.Count == 0)
        {
            return BadRequest();
        }

        if (await context.Vulnerabilities.CountAsync() == 0)
        {
            return Ok("Угроз не обнаружено");
        }

        var hostVulnerabilities = new Dictionary<int, int>();

        foreach (var host in hosts)
        {
            var scan = await generator.Generate(host);
            await context.Scans.AddAsync(scan);
            await context.SaveChangesAsync();
            hostVulnerabilities.Add(host.Id, scan.ScanVulnerability.Select(sv=>sv.Vulnerability).ToList().Count);
        }

        var report = "Обнаружено уязвимостей:\n" + string.Join("\n", hostVulnerabilities
            .Select(hv => $"Хост {hv.Key} — {hv.Value} уязвимостей"));
        return Ok(report);
    }
}