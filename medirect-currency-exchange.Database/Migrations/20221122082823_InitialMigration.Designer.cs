// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using medirect_currency_exchange.Database.Context;

#nullable disable

namespace medirectcurrencyexchange.Database.Migrations
{
    [DbContext(typeof(CurrencyExchangeDbContext))]
    [Migration("20221122082823_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.CurrencyExchangeTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ConvertedAmount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("FromCurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SourceAmount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("ToCurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CurrencyExchangeTransactions");
                });

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.Customer", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.CustomerWallet", b =>
                {
                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomerId", "CurrencyCode");

                    b.ToTable("CustomerWallets");
                });

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.CurrencyExchangeTransaction", b =>
                {
                    b.HasOne("medirect_currency_exchange.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.CustomerWallet", b =>
                {
                    b.HasOne("medirect_currency_exchange.Domain.Models.Customer", "Customer")
                        .WithMany("Wallets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("medirect_currency_exchange.Domain.Models.Customer", b =>
                {
                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
