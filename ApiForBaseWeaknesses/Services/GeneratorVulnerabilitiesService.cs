using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Services;

public class GeneratorVulnerabilitiesService(AppDbContext context)
{

    public async Task<Scan> Generate(Host host,int maxVulnerabilities)
    {
        var random = new Random();
        var randomVulnerabilityIds = new List<int>();
        
        var amountDetectedVulnerabilities = random.Next(maxVulnerabilities);
        var allVulnerabilityIds = await context.Vulnerabilities.Select(v => v.Id).ToListAsync();
        var shuffledVulnerabilityIds = allVulnerabilityIds.OrderBy(_ => random.Next()).ToList();
        
        for (var i=0;i <= amountDetectedVulnerabilities-1;i++)
        {
            randomVulnerabilityIds.Add(shuffledVulnerabilityIds[i]);
        }
        
        var scan = new Scan
        {
            ScannedAt = DateTime.UtcNow.Date,
            HostId = host.Id
        };

        scan.ScanVulnerability = randomVulnerabilityIds.Select(index => new ScanVulnerability
        {
            VulnerabilityId = index,
            Scan = scan
        }).ToList();
        return scan;
    }
}