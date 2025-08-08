using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Requests;
using ApiForBaseWeaknesses.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scan = ApiForBaseWeaknesses.Responses.ForScans.Scan;

namespace ApiForBaseWeaknesses.Controllers;

[ApiController]
[Route("[controller]")]
public class ScansController
    : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ScanService _generator;

    public ScansController(AppDbContext context, IMapper mapper, ScanService generator)
    {
        _context = context;
        _mapper = mapper;
        _generator = generator;
    }

    [HttpGet]
    public async Task<ActionResult> GetScans([FromQuery] Requests.Scan scan)
    {
        IQueryable<Models.Scan> validScans;
        if (scan.StartDate > scan.EndDate)
        {
            return BadRequest();
        }

        if (scan.StartDate != null && scan.EndDate != null)
        {
            validScans = _context.Scans
                .Where(s => s.ScannedAt >= scan.StartDate && s.ScannedAt <= scan.EndDate)
                .OrderBy(s => s.ScannedAt);
        }
        else
        {
            validScans = _context.Scans
                .Where(s =>
                    (!scan.StartDate.HasValue || s.ScannedAt >= scan.StartDate.Value) &&
                    (!scan.EndDate.HasValue || s.ScannedAt <= scan.EndDate.Value)
                )
                .OrderBy(s => s.ScannedAt);
        }

        var orderScans = scan.SortMode switch
        {
            "Id" => scan.SortDescending ? validScans.OrderByDescending(i => i.Id) : validScans.OrderBy(i=>i.Id),
            "Date" => scan.SortDescending ? validScans.OrderByDescending(i=>i.ScannedAt) : validScans.OrderBy(i=>i.ScannedAt),
            _ => null
        };
        if (orderScans == null)
        {
            return BadRequest();
        }
        
        var scans = orderScans
            .Skip((scan.Page - 1) * scan.PageSize).Take(scan.PageSize);
        var scanDtos = await scans.ProjectTo<Scan>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(scanDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetScan(int id)
    {
        var scan = await _context.Scans.FirstOrDefaultAsync(s => s.Id == id);
        if (scan == null)
        {
            return NotFound();
        }

        var scanResponseDto = await _context.Scans
            .Where(s => s.Id == id)
            .ProjectTo<Scan>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return Ok(scanResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult> Run(HostIndexes hostId)
    {
             var hosts = await _context.Hosts.Where(h => hostId.Indexes.ToList()
                .Contains(h.Id)).ToListAsync();
        
        if (hosts.Count == 0 || hosts.Count != hostId.Indexes.Count)
        {
            return BadRequest();
        }

        var hostVulnerabilities = new Dictionary<int, int>();

        foreach (var host in hosts)
        {
            var scan = await _generator.Start(host);
            await _context.Scans.AddAsync(scan);
            await _context.SaveChangesAsync();
            hostVulnerabilities.Add(host.Id, scan.ScanVulnerability.Select(sv => sv.Vulnerability).ToList().Count);
        }

        var report = "Обнаружено уязвимостей:\n" + string.Join("\n", hostVulnerabilities
            .Select(hv => $"Хост {hv.Key} — {hv.Value} уязвимостей"));
        return Ok(report);
    }
}