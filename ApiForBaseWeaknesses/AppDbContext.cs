using ApiForBaseWeaknesses.Models;
using Microsoft.EntityFrameworkCore;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vulnerability> Vulnerabilities { get; set; }
    public DbSet<Reference> References { get; set; }
    public DbSet<CvssMetric> CvssMetrics { get; set; }
    public DbSet<Host> Hosts { get; set; }
    public DbSet<Scan> Scans { get; set; }
    public DbSet<ScanVulnerability> ScanVulnerabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CvssMetric>().HasKey(c => c.Id);
        modelBuilder.Entity<CvssMetric>().Property(c => c.Id).HasColumnName("id");
        modelBuilder.Entity<CvssMetric>().ToTable("cvss_metrics")
            .Property(c => c.Version).IsRequired().HasMaxLength(3).HasColumnName("version");
        modelBuilder.Entity<CvssMetric>().Property(c => c.Vector).IsRequired()
            .HasColumnName("vector");
        modelBuilder.Entity<CvssMetric>().Property(c => c.BaseScore).HasColumnName("base_score");
        modelBuilder.Entity<CvssMetric>().Property(c => c.VulnerabilityId).HasColumnName("vulnerability_id");
        modelBuilder.Entity<CvssMetric>().ToTable("cvss_metrics");
        modelBuilder.Entity<CvssMetric>().HasOne(c => c.Vulnerability).WithMany(v => v.CvssMetrics);

        modelBuilder.Entity<Reference>().HasKey(r => r.Id);
        modelBuilder.Entity<Reference>().Property(r => r.Id).HasColumnName("id");
        modelBuilder.Entity<Reference>().Property(r => r.VulnerabilityId).HasColumnName("vulnerability_id");
        modelBuilder.Entity<Reference>().Property(r => r.Url).HasColumnName("url");
        modelBuilder.Entity<Reference>().Property(r => r.Source).HasColumnName("source");
        modelBuilder.Entity<Reference>().ToTable("references");
        modelBuilder.Entity<Reference>().HasOne(r => r.Vulnerability).WithMany(v => v.References);

        modelBuilder.Entity<Vulnerability>().HasKey(v => v.Id);
        modelBuilder.Entity<Vulnerability>().Property(v => v.Id).HasColumnName("id");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Published).HasColumnName("published");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Status).HasColumnName("status");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Name).HasColumnName("name");
        modelBuilder.Entity<Vulnerability>().Property(v => v.Description).HasColumnName("description");
        modelBuilder.Entity<Vulnerability>().HasMany(v => v.CvssMetrics).WithOne(c => c.Vulnerability)
            .HasForeignKey(с => с.VulnerabilityId);
        modelBuilder.Entity<Vulnerability>().HasMany(v => v.References).WithOne(r => r.Vulnerability)
            .HasForeignKey(r => r.VulnerabilityId);
        modelBuilder.Entity<Vulnerability>().ToTable("vulnerabilities");

        modelBuilder.Entity<Host>().HasKey(h => h.Id);
        modelBuilder.Entity<Host>().Property(h => h.Id).HasColumnName("id");
        modelBuilder.Entity<Host>().Property(h => h.Ip).HasColumnName("ip").IsRequired();
        modelBuilder.Entity<Host>().HasIndex(h => h.Ip).IsUnique();
        modelBuilder.Entity<Host>().Property(h => h.Description).HasColumnName("description");
        modelBuilder.Entity<Host>().Property(h => h.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<Host>().ToTable("hosts");
        modelBuilder.Entity<Host>().HasMany(h => h.Scans).WithOne(s => s.Host);
        
        modelBuilder.Entity<Scan>().HasKey(s => s.Id);
        modelBuilder.Entity<Scan>().Property(s => s.Id).HasColumnName("id");
        modelBuilder.Entity<Scan>().Property(s => s.HostId).HasColumnName("host_id");
        modelBuilder.Entity<Scan>().Property(s => s.ScannedAt).HasColumnName("scanned_at");
        modelBuilder.Entity<Scan>().ToTable("scans");
        modelBuilder.Entity<Scan>().HasOne(s => s.Host).WithMany(h => h.Scans);
        modelBuilder.Entity<Scan>().HasMany(s => s.ScanVulnerability).WithOne(sv => sv.Scan);
        
        modelBuilder.Entity<ScanVulnerability>().HasKey(sv => new { sv.ScanId, sv.VulnerabilityId });
        modelBuilder.Entity<ScanVulnerability>().Property(sv => sv.ScanId).HasColumnName("scan_id");
        modelBuilder.Entity<ScanVulnerability>().Property(sv => sv.VulnerabilityId).HasColumnName("vulnerability_id");
        modelBuilder.Entity<ScanVulnerability>().ToTable("scan_vulnerability");
        modelBuilder.Entity<ScanVulnerability>().HasOne(sv => sv.Scan).WithMany(s => s.ScanVulnerability)
            .HasForeignKey(sv => sv.ScanId);
    }
}