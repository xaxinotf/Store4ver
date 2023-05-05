using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Store444.Models;

namespace Store444.Contexts;

public partial class DrugShopContext : DbContext
{
    public DrugShopContext()
    {
    }

    public DrugShopContext(DbContextOptions<DrugShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShipType> ShipTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server= (LocalDB)\\MSSQLLocalDB;Database=DrugShop; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.ShipTypeId, "IX_Orders_DeliveryId");

            entity.HasIndex(e => e.PaymentTypeId, "IX_Orders_PaymentTypeId");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.Property(e => e.DeliveryAddress).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("FK_Orders_PaymentType");

            entity.HasOne(d => d.ShipTypeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipTypeId)
                .HasConstraintName("FK_Orders_ShipType");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.ToTable("PaymentType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Desription).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(d => d.Orders).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProduct",
                    r => r.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_OrderProduct_Orders_OrdersOrderId"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_OrderProduct_Products_NameProductId"),
                    j =>
                    {
                        j.HasKey("ProductId", "OrderId");
                        j.ToTable("OrderProduct");
                        j.HasIndex(new[] { "OrderId" }, "IX_OrderProduct_OrdersOrderId");
                    });
        });

        modelBuilder.Entity<ShipType>(entity =>
        {
            entity.ToTable("ShipType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
