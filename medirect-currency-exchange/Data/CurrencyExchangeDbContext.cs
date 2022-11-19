using medirect_currency_exchange.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace medirect_currency_exchange.Data
{
	public class CurrencyExchangeDbContext : DbContext
	{
		public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options) : base(options)
		{
			
		}
		
		DbSet<Customer> Customers { get; set; }
		DbSet<CustomerWallet> CustomerWallets { get; set; }
		DbSet<CurrencyExchangeHistory> CurrencyExchangeHistories { get; set; }

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
