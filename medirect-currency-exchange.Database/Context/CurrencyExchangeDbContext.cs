using medirect_currency_exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace medirect_currency_exchange.Database.Context
{
	public class CurrencyExchangeDbContext : DbContext
	{
		public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options) : base(options)
		{
			
		}
		
		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerWallet> CustomerWallets { get; set; }
		public DbSet<CurrencyExchangeHistory> CurrencyExchangeHistories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CurrencyExchangeHistory>()
				.HasOne(s => s.SourceWallet)
				.WithOne()
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<CurrencyExchangeHistory>()
				.HasOne(s => s.TargetWallet)
				.WithOne()
				.OnDelete(DeleteBehavior.Restrict);
		}

	}
}
