using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class ScansController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScansController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
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
            return NotFound();
        }
        return Ok(scans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOne(int id)
    {
        // var scan = await _context.Scans
        //     .Include(s => s.ScanVulnerability)
        //     .ThenInclude(sv => sv.Vulnerability).ThenInclude(v => v.CvssMetrics)
        //     .Include(s => s.ScanVulnerability).ThenInclude(sv => sv.Vulnerability)
        //     .ThenInclude(v => v.References).Select(s => new
        //     {
        //         s.Id,
        //         s.ScannedAt,
        //         s.HostId,
        //         Vulnerabilities = s.ScanVulnerability.Select(sv => new
        //         {
        //             sv.Vulnerability
        //         }).ToList()
        //     }).ToListAsync();
        var scan = _context.Scans.Where(s=>s.Id==id)
            .Select(s => new
            {
                s.Id,
                s.ScannedAt,
                Vulnerabilities = s.ScanVulnerability.Select(sv => new
                {
                    sv.Vulnerability.Published,
                    sv.Vulnerability.Status,
                    sv.Vulnerability.Name,
                    sv.Vulnerability.Description,
                    Metrics = sv.Vulnerability.CvssMetrics.Select(cm => new
                    {
                        cm.Version,
                        cm.Vector,
                        cm.BaseScore
                    }),
                    Reference = sv.Vulnerability.References.Select(r => new
                    {
                        r.Url,
                        r.Source,
                    })
                })
            }).ToList();
        if (scan.Count == 0)
        {
            return NotFound();
        }

        return Ok(scan);
    }
}