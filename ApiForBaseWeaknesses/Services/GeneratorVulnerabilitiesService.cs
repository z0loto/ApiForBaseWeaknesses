using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Services;

public class GeneratorVulnerabilitiesService(AppDbContext context, ILogger<GeneratorVulnerabilitiesService> logger)
{
    private readonly ILogger<GeneratorVulnerabilitiesService> _logger = logger;

    public async Task<Scan> Generate(Host host)
    {
        var random = new Random();
        var allVulnerabilityIds = await context.Vulnerabilities.Select(v => v.Id).ToListAsync();
        var shuffledIds = allVulnerabilityIds.OrderBy(x => random.Next()).ToList();
        var scan = new Scan
        {
            ScannedAt = DateTime.UtcNow.Date,
            HostId = host.Id
        };

        scan.ScanVulnerability = shuffledIds.Select(index => new ScanVulnerability
        {
            VulnerabilityId = index,
            Scan = scan
        }).ToList();
        return scan;
    }
}