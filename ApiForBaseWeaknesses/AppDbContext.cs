using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    public DbSet<Vulnerability> Vulnerabilitys { get; set; }
    public DbSet<Reference> References { get; set; }
    public DbSet<CvssMetric> Cvss_Metrics { get; set; }
}