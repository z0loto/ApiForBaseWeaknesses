using ApiForBaseWeaknesses.Dtos.Vulnerabilities;
using ApiForBaseWeaknesses.Requests;
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
    
    [HttpGet("top-vulnerabilities/{count}")]
    public async Task<IActionResult> GetTopThreats([FromRoute]int count)
    {
        var topVulnerabilities = await _context.ScanVulnerabilities
            .GroupBy(sv => sv.VulnerabilityId).Select(group => new
            {
                VulnerabilityId = group.Key,
                Count = group.Count()
            }).OrderByDescending(x => x.Count).Take(count)
            .Join(_context.Vulnerabilities,g => g.VulnerabilityId, 
                v => v.Id,
                (g, v) => new TopVulnerabilities()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    Status = v.Status,
                    Published = v.Published,
                    Count = g.Count
                })
            .ToListAsync();

        return Ok(topVulnerabilities);
    }
    [HttpGet("top-hosts/{count}")]
    public async Task<IActionResult> GetTopHostsByVulnerability(int count)
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
            .Select(x => new TopHosts
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
            .Take(count)
            .ToListAsync();

        return Ok(topHosts);
    }
    [HttpGet("clear-hosts")]
    public async Task<IActionResult> GetHostsWithoutVulnerabilities()
    {
        var hostsWithoutVulns = await _context.Hosts
            .Where(h => !h.Scans
                .SelectMany(s => s.ScanVulnerability)
                .Any())
            .Select(h => new TopHosts
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