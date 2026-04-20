using System.Data.Entity;
using CarteMinistre2026.Models;

namespace CarteMinistre2026.Data
{
    public class AppDbContext : DbContext
    {
        // Le nom doit correspondre à celui dans App.config
        public AppDbContext() : base("name=CardPrintDbConnection")
        {
            // Crée automatiquement la base si elle n'existe pas
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PrintHistory> PrintHistories { get; set; }
    }
}