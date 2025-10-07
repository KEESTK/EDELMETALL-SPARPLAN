using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sparplan.Infrastructure.Persistence
{
    /// <summary>
    /// Factory zur Erstellung des <see cref="AppDbContext"/> 
    /// für Design-Time-Operationen von Entity Framework Core 
    /// (z. B. Migrationserstellung über "dotnet ef migrations add").
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Erstellt eine neue Instanz des <see cref="AppDbContext"/> 
        /// mit einem Standard-ConnectionString für Design-Time.
        /// </summary>
        /// <param name="args">Von EF übergebene Argumente (werden hier nicht genutzt).</param>
        /// <returns>Eine konfigurierte <see cref="AppDbContext"/>-Instanz.</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Standard-ConnectionString für Migrationen (Dummy-Werte für EF Design-Time).
            // In Runtime-Szenarien wird der ConnectionString über Environment-Variablen bereitgestellt.
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=sparplan_db;Username=sparplan;Password=sparplan_pw"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}