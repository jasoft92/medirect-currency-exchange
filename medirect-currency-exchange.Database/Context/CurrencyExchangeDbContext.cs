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

			modelBuilder.Entity<Customer>().HasData(
				new Customer { Id = 1, Name = "Joseph", Surname = "Attard", IdCard = "13392G", Email = "jos.att@gmail.com" },
				new Customer { Id = 2, Name = "Roger", Surname = "Federer", IdCard = "11223S", Email = "rog.fed@gmail.com" },
				new Customer { Id = 3, Name = "Rafael", Surname = "Nadal", IdCard = "12121S", Email = "raf.nad@gmail.com" },
				new Customer { Id = 4, Name = "Andy", Surname = "Murray", IdCard = "12312E", Email = "and.mur@gmail.com" },
				new Customer { Id = 5, Name = "Novak", Surname = "Djokovic", IdCard = "54321S", Email = "nov.djo@gmail.com" });

			modelBuilder.Entity<CustomerWallet>()
				.HasKey(m => new { m.CustomerId, m.CurrencyCode });

			modelBuilder.Entity<CustomerWallet>().HasData(
				new CustomerWallet { CustomerId = 1, CurrencyCode = "EUR", Amount = 1000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 1, CurrencyCode = "GBP", Amount = 500, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 2, CurrencyCode = "EUR", Amount = 5000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 2, CurrencyCode = "CHF", Amount = 10000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 3, CurrencyCode = "EUR", Amount = 6000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 3, CurrencyCode = "USD", Amount = 8000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 4, CurrencyCode = "EUR", Amount = 6000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 4, CurrencyCode = "GBP", Amount = 8000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 5, CurrencyCode = "EUR", Amount = 6000, LastModified = DateTime.Now },
				new CustomerWallet { CustomerId = 5, CurrencyCode = "AUD", Amount = 11000, LastModified = DateTime.Now });

			modelBuilder.Entity<CurrencyExchangeTransaction>()
				.HasKey(m => m.Id);

			modelBuilder.Entity<CurrencyExchangeTransaction>()
				.Property(p => p.Id)
				.ValueGeneratedOnAdd();


		}

	}
}
