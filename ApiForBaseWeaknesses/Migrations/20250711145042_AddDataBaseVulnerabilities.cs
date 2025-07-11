using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiForBaseWeaknesses.Migrations
{
    /// <inheritdoc />
    public partial class AddDataBaseVulnerabilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hosts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hosts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vulnerabilities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    published = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vulnerabilities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scans",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    host_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scans", x => x.id);
                    table.ForeignKey(
                        name: "FK_scans_hosts_host_id",
                        column: x => x.host_id,
                        principalTable: "hosts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cvss_metrics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    vector = table.Column<string>(type: "text", nullable: false),
                    base_score = table.Column<double>(type: "double precision", nullable: true),
                    vulnerability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cvss_metrics", x => x.id);
                    table.ForeignKey(
                        name: "FK_cvss_metrics_vulnerabilities_vulnerability_id",
                        column: x => x.vulnerability_id,
                        principalTable: "vulnerabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "references",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    vulnerability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_references", x => x.id);
                    table.ForeignKey(
                        name: "FK_references_vulnerabilities_vulnerability_id",
                        column: x => x.vulnerability_id,
                        principalTable: "vulnerabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scan_vulnerability",
                columns: table => new
                {
                    scan_id = table.Column<int>(type: "integer", nullable: false),
                    vulnerability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scan_vulnerability", x => new { x.scan_id, x.vulnerability_id });
                    table.ForeignKey(
                        name: "FK_scan_vulnerability_scans_scan_id",
                        column: x => x.scan_id,
                        principalTable: "scans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scan_vulnerability_vulnerabilities_vulnerability_id",
                        column: x => x.vulnerability_id,
                        principalTable: "vulnerabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cvss_metrics_vulnerability_id",
                table: "cvss_metrics",
                column: "vulnerability_id");

            migrationBuilder.CreateIndex(
                name: "IX_hosts_ip",
                table: "hosts",
                column: "ip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_references_vulnerability_id",
                table: "references",
                column: "vulnerability_id");

            migrationBuilder.CreateIndex(
                name: "IX_scan_vulnerability_vulnerability_id",
                table: "scan_vulnerability",
                column: "vulnerability_id");

            migrationBuilder.CreateIndex(
                name: "IX_scans_host_id",
                table: "scans",
                column: "host_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cvss_metrics");

            migrationBuilder.DropTable(
                name: "references");

            migrationBuilder.DropTable(
                name: "scan_vulnerability");

            migrationBuilder.DropTable(
                name: "scans");

            migrationBuilder.DropTable(
                name: "vulnerabilities");

            migrationBuilder.DropTable(
                name: "hosts");
        }
    }
}
