using ApiForBaseWeaknesses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class ScanExecutionController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScanExecutionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> Scanning(List<int> indexes)
    {
        var hosts = await _context.Hosts.Where(h => indexes.Contains(h.Id)).ToListAsync();
        if (hosts.Count != 0)
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
            Dictionary<int, int> hostVulnerabilities = new();
            foreach (var host in hosts)
            {
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
                var scanVulnerabilities = validIndexes.Select(index => new ScanVulnerability
                {
                    ScanId = scan.Id,
                    VulnerabilityId = index
                }).ToList();
                await _context.ScanVulnerabilities.AddRangeAsync(scanVulnerabilities);
                await _context.SaveChangesAsync();
                hostVulnerabilities.Add(host.Id, scanVulnerabilities.Count);
            }

            string report = "Обнаружено уязвимостей:\n" + string.Join("\n", hostVulnerabilities
                .Select(hv => $"Хост {hv.Key} — {hv.Value} уязвимостей"));
            return Ok(report);
        }

        return NotFound();
    }
}