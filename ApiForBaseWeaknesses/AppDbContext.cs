using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiForBaseWeaknesses;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CvssMetric>().ToTable("Cvss_Metrics")
            .Property(c=>c.Id).IsRequired().HasMaxLength(3);
        modelBuilder.Entity<CvssMetric>().Property(c => c.VectorString).IsRequired()
            .HasColumnName("Vector_String");
        modelBuilder.Entity<CvssMetric>().Property(c => c.BaseScore).HasColumnName("Base_Score");
        modelBuilder.Entity<CvssMetric>().Property(c => c.VulnerabilityId).HasColumnName("Vulnerability_Id");
        modelBuilder.Entity<Reference>().Property(c => c.VulnerabilityId).HasColumnName("Vulnerability_Id");

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Vulnerability> Vulnerabilities { get; set; }
    public DbSet<Reference> References { get; set; }
    public DbSet<CvssMetric> CvssMetrics { get; set; }//изменено с Cvss_Metrics
}