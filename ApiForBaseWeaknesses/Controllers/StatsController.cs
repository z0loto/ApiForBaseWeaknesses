using ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

public class StatsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StatsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("top-vulnerabilities")]
    public async Task<IActionResult> GetTopThreats()
    {
        var topThreats = await _context.ScanVulnerabilities
            .GroupBy(sv => sv.VulnerabilityId).Select(group => new
            {
                VulnerabilityId = group.Key,
                Count = group.Count()
            }).OrderByDescending(x => x.Count).Take(10)
            .Join(_context.Vulnerabilities,g => g.VulnerabilityId, 
                v => v.Id,
                (g, v) => new Dtos.VulnerabilittDto.Top()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    Status = v.Status,
                    Published = v.Published,
                    Count = g.Count
                })
            .ToListAsync();

        return Ok(topThreats);
    }
    [HttpGet("top-hosts-by-vulnerabilities")]
    public async Task<IActionResult> GetTopHostsByVulnerability()
    {
        var topHosts = await _context.Hosts
            .Select(h => new
            {
                Host = h,
                LastScan = h.Scans
                    .OrderByDescending(s => s.ScannedAt)
                    .FirstOrDefault()
            })
            .Where(x => x.LastScan != null)
            .Select(x => new Top
            {
                Id = x.Host.Id,
                Ip = x.Host.Ip,
                Description = x.Host.Description,
                CreatedAt = x.Host.CreatedAt,
                VulnerabilityCount = x.LastScan.ScanVulnerability
                    .Select(sv => sv.VulnerabilityId)
                    .Distinct()
                    .Count()
            })
            .Where(t => t.VulnerabilityCount > 0)
            .OrderByDescending(t => t.VulnerabilityCount)
            .Take(5)
            .ToListAsync();

        return Ok(topHosts);
    }
    [HttpGet("hosts-without-vulnerabilities")]
    public async Task<IActionResult> GetHostsWithoutVulnerabilities()
    {
        var hostsWithoutVulns = await _context.Hosts
            .Where(h => !h.Scans
                .SelectMany(s => s.ScanVulnerability)
                .Any())
            .Select(h => new Top
            {
                Id=h.Id,
                Ip=h.Ip,
                Description=h.Description,
                CreatedAt = h.CreatedAt
            })
            .ToListAsync();

        return Ok(hostsWithoutVulns);
    }

}