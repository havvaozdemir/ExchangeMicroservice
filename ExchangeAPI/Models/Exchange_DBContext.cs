using Microsoft.EntityFrameworkCore;

namespace ExchangeAPI.Models
{
    public partial class Exchange_DBContext : DbContext
    {
        public Exchange_DBContext()
        {
        }

        public Exchange_DBContext(DbContextOptions<Exchange_DBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        public DbSet<CustomerCurrency> CustomerCurrencies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerCurrency>(entity =>
            {
                entity.ToTable("CustomerCurrency");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}