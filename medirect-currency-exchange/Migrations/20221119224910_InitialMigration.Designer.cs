﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using medirect_currency_exchange.Data;

#nullable disable

namespace medirectcurrencyexchange.Migrations
{
    [DbContext(typeof(CurrencyExchangeDbContext))]
    [Migration("20221119224910_InitialMigration")]
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

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.CurrencyExchangeHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,5)");

                    b.Property<decimal>("SourceAmount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<Guid>("SourceWalletId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TargetAmount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<Guid>("TargetWalletId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SourceWalletId")
                        .IsUnique();

                    b.HasIndex("TargetWalletId")
                        .IsUnique();

                    b.ToTable("CurrencyExchangeHistories");
                });

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

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

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.CustomerWallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerWallets");
                });

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.CurrencyExchangeHistory", b =>
                {
                    b.HasOne("medirect_currency_exchange.Models.Domain.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("medirect_currency_exchange.Models.Domain.CustomerWallet", "SourceWallet")
                        .WithOne()
                        .HasForeignKey("medirect_currency_exchange.Models.Domain.CurrencyExchangeHistory", "SourceWalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("medirect_currency_exchange.Models.Domain.CustomerWallet", "TargetWallet")
                        .WithOne()
                        .HasForeignKey("medirect_currency_exchange.Models.Domain.CurrencyExchangeHistory", "TargetWalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("SourceWallet");

                    b.Navigation("TargetWallet");
                });

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.CustomerWallet", b =>
                {
                    b.HasOne("medirect_currency_exchange.Models.Domain.Customer", "Customer")
                        .WithMany("Wallets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("medirect_currency_exchange.Models.Domain.Customer", b =>
                {
                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
