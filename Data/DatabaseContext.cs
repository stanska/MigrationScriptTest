using Microsoft.EntityFrameworkCore;
using MigrationScripTest.table;
using MigrationScripTest.view;
using System.Diagnostics;

namespace MigrationScripTest.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        //public DbSet<Blog> Blogs => Set<Blog>();
        public DbSet<Blog> Blog { get; set; }
        //public DbSet<ExpenseByTotal> ExpenseTotals { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<ExpenseHistory> ExpenseHistory { get; set; }
        public DbSet<ExpenseByTotal> ExpenseTotal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
                Debug.WriteLine(connectionString);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
               .Entity<ExpenseByTotal>()
               .ToView("ExpenseByTotalView")
               .HasKey(t => t.Id);

            modelBuilder.Entity<ExpenseItem>().HasData(
                new ExpenseItem() { Id = 1, Name = "Ferrari", Category = "Big Expense" },
                new ExpenseItem() { Id = 2, Name = "Cheese", Category = "Small Expense" },
                new ExpenseItem() { Id = 3, Name = "TV", Category = "Mid Expense" }
                );
        }
    }
}
