using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiForBaseWeaknesses;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CvssMetric
        modelBuilder.Entity<CvssMetric>().HasKey(c => c.Id);
        modelBuilder.Entity<CvssMetric>().ToTable("Cvss_Metrics")
            .Property(c=>c.Version).IsRequired().HasMaxLength(3).HasColumnName("Version");
        modelBuilder.Entity<CvssMetric>().Property(c => c.VectorString).IsRequired()
            .HasColumnName("Vector_String");
        modelBuilder.Entity<CvssMetric>().Property(c => c.BaseScore).HasColumnName("Base_Score");
        modelBuilder.Entity<CvssMetric>().Property(c => c.VulnerabilityId).HasColumnName("Vulnerability_Id");
        // Reference
        modelBuilder.Entity<Reference>().HasKey(r => r.Id);
        modelBuilder.Entity<Reference>().Property(r => r.VulnerabilityId).HasColumnName("Vulnerability_Id");
        modelBuilder.Entity<Reference>().Property(r => r.Url).HasColumnName("Url");
        modelBuilder.Entity<Reference>().Property(r => r.Source).HasColumnName("Source");
        // Vulnerabilities
        modelBuilder.Entity<Vulnerability>().HasKey(v => v.Id);
        modelBuilder.Entity<Vulnerability>().Property(v => v.Published).HasColumnName("Published");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Status).HasColumnName("Status");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Name).HasColumnName("Name");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Description).HasColumnName("Description");
        modelBuilder.Entity<Vulnerability>().HasMany(v => v.CvssMetrics).WithOne(c => c.Vulnerability)
            .HasForeignKey(с => с.VulnerabilityId);
        modelBuilder.Entity<Vulnerability>().HasMany(v => v.References).WithOne(r => r.Vulnerability)
            .HasForeignKey(r => r.VulnerabilityId);
        base.OnModelCreating(modelBuilder);
        //NetworkHost
        modelBuilder.Entity<NetworkHost>().HasKey(h=>h.Id);
        modelBuilder.Entity<NetworkHost>().Property(h => h.Ip).HasColumnName("Ip").IsRequired();
        modelBuilder.Entity<NetworkHost>().HasIndex(h => h.Ip).IsUnique();
    }
    public DbSet<Vulnerability> Vulnerabilities { get; set; }
    public DbSet<Reference> References { get; set; }
    public DbSet<CvssMetric> CvssMetrics { get; set; }
    public DbSet<NetworkHost> Host { get; set; }
}