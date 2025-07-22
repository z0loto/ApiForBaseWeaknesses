using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Services;

public class ScanService
{
    private readonly AppDbContext _context;

    public ScanService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Scan> Start(Host host)
    {
        var random = new Random();
        var randomVulnerabilityIds = new List<int>();
        var maxVulnerabilities = await _context.Vulnerabilities.CountAsync();
        
        var amountDetectedVulnerabilities = random.Next(maxVulnerabilities);
        var allVulnerabilityIds = await _context.Vulnerabilities.Select(v => v.Id).ToListAsync();
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