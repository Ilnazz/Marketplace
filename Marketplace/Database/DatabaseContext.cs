using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Marketplace.Database.Models;

namespace Marketplace.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankCard> BankCards { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientProduct> ClientProducts { get; set; }

    public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductManufacturer> ProductManufacturers { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<Salesman> Salesmen { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Marketplace2;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankCard>(entity =>
        {
            entity.ToTable("BankCard");

            entity.Property(e => e.ExpirationDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Number)
                .HasMaxLength(19)
                .IsFixedLength();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.HasOne(d => d.BankCard).WithMany(p => p.Clients)
                .HasForeignKey(d => d.BankCardId)
                .HasConstraintName("FK_Client_BankCard");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_User");
        });

        modelBuilder.Entity<ClientProduct>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.ProductId });

            entity.ToTable("Client_Product");

            entity.HasOne(d => d.Client).WithMany(p => p.Basket)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Product_Client");

            entity.HasOne(d => d.Product).WithMany(p => p.ClientProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Product_Product");
        });

        modelBuilder.Entity<DeliveryPoint>(entity =>
        {
            entity.ToTable("DeliveryPoint");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.DateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("date");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Client");

            entity.HasOne(d => d.DeliveryPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryPointId)
                .HasConstraintName("FK_Order_DeliveryPoint");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.ToTable("Order_Product");

            entity.Property(e => e.Cost).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductManufacturer");

            entity.HasOne(d => d.Salesman).WithMany(p => p.Products)
                .HasForeignKey(d => d.SalesmanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Salesman");
        });

        modelBuilder.Entity<ProductManufacturer>(entity =>
        {
            entity.ToTable("ProductManufacturer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.ToTable("ProductPhoto");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<Salesman>(entity =>
        {
            entity.ToTable("Salesman");

            entity.HasOne(d => d.User).WithMany(p => p.Salesmen)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salesman_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Login).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
