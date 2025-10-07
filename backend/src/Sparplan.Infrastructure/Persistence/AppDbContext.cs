using Microsoft.EntityFrameworkCore;
using Sparplan.Domain.Entities;

namespace Sparplan.Infrastructure.Persistence
{
    /// <summary>
    /// Zentrale Datenbank-Kontextklasse für Entity Framework Core.
    /// Definiert die DbSets und Konfigurationen für Sparpläne, Transaktionen und Depots.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Erstellt eine neue Instanz des Datenbankkontexts.
        /// </summary>
        /// <param name="options">Von außen bereitgestellte Konfigurationsoptionen (z. B. Connection String).</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Tabelle für Sparpläne.
        /// </summary>
        public DbSet<SparplanClass> Sparplaene { get; set; }

        /// <summary>
        /// Tabelle für Transaktionen.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Tabelle für Depots.
        /// </summary>
        public DbSet<Depot> Depots { get; set; }

        public DbSet<MetalPriceHistory> MetalPriceHistories { get; set; }

        /// <summary>
        /// Konfiguration der Entitäten und ihrer Beziehungen.
        /// </summary>
        /// <param name="modelBuilder">Der von EF Core bereitgestellte ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primärschlüssel definieren
            modelBuilder.Entity<SparplanClass>().HasKey(s => s.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Depot>().HasKey(d => d.Id);
            modelBuilder.Entity<MetalPriceHistory>().HasKey(p => p.Id);

            // Beziehung: Sparplan gehört zu genau einem Depot
            modelBuilder.Entity<SparplanClass>()
                .HasOne(s => s.Depot)
                .WithMany(d => d.Sparplaene)
                .HasForeignKey(s => s.DepotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Beziehung: Transaktion gehört zu genau einem Sparplan
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Sparplan)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.SparplanClassId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<MetalPriceHistory>()
                .HasIndex(p => new { p.Metal, p.Date })
                .IsUnique();

            modelBuilder.Entity<MetalPriceHistory>()
                .Property(p => p.PricePerUnit)
                .HasPrecision(18, 6);

            base.OnModelCreating(modelBuilder);
            }
        }
}
