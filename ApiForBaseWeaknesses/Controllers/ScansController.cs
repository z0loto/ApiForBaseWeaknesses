using ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;
using ApiForBaseWeaknesses.Dtos.ScanDtos.Requests;
using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Request = ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto.Request;
using Scan = ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos.Scan;

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
    public async Task<ActionResult> GetScans([FromQuery] Dtos.ScanDtos.Requests.Request request)
    {
        IQueryable<Models.Scan> validScans;
        if (request.StartDate > request.EndDate)
        {
            return BadRequest();
        }

        if (request.StartDate != null && request.EndDate != null)
        {
            validScans = _context.Scans
                .Where(s => s.ScannedAt >= request.StartDate && s.ScannedAt <= request.EndDate)
                .OrderBy(s => s.ScannedAt);
        }
        else
        {
            validScans = _context.Scans
                .Where(s =>
                    (!request.StartDate.HasValue || s.ScannedAt >= request.StartDate.Value) &&
                    (!request.EndDate.HasValue || s.ScannedAt <= request.EndDate.Value)
                )
                .OrderBy(s => s.ScannedAt);
        }
        validScans = request.SortMode switch
        {
            SortMode.Date when !request.SortDescending => validScans.OrderBy(i => i.ScannedAt),
            SortMode.Date when request.SortDescending => validScans.OrderByDescending(i => i.ScannedAt),

            SortMode.Id when !request.SortDescending => validScans.OrderBy(i => i.Id),
            SortMode.Id when request.SortDescending => validScans.OrderByDescending(i => i.Id),

            _ => validScans
        };

        var scans = validScans
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
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
    public async Task<ActionResult> Run(Request indexes)
    {
             var hosts = await _context.Hosts.Where(h => indexes.Indexes.ToList()
                .Contains(h.Id)).ToListAsync();
        
        if (hosts.Count == 0)
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