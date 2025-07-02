using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    public DbSet<Vulnerability> Vulnerability { get; set; }
    public DbSet<References> References { get; set; }
    public DbSet<CvssMetric> Cvss_Metrics { get; set; }
}