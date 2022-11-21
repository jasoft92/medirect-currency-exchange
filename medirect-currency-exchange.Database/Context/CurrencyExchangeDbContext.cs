using medirect_currency_exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace medirect_currency_exchange.Database.Context
{
	public class CurrencyExchangeDbContext : DbContext
	{
		public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options) : base(options) { }

		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerWallet?> CustomerWallets { get; set; }
		public DbSet<CurrencyExchangeTransaction> CurrencyExchangeTransactions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Customer>()
				.HasKey(m => m.Id);

			modelBuilder.Entity<Customer>()
				.Property(p => p.Id)
				.ValueGeneratedNever();

			modelBuilder.Entity<CustomerWallet>()
				.HasKey(m => new { m.CustomerId, m.CurrencyCode });

			modelBuilder.Entity<CurrencyExchangeTransaction>()
				.HasKey(m => m.Id);

			modelBuilder.Entity<CurrencyExchangeTransaction>()
				.Property(p => p.Id)
				.ValueGeneratedOnAdd();
		}

	}
}
