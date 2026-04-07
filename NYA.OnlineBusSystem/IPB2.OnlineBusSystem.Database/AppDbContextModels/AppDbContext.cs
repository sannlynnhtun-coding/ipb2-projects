using System;
using System.Collections.Generic;
using IPB2.OnlineBusSystem.Database.Common;
using Microsoft.EntityFrameworkCore;

namespace IPB2.OnlineBusSystem.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(ConnectionString.GetConnection());
        }
    }
    public virtual DbSet<TblBook> TblBooks { get; set; }

    public virtual DbSet<TblBusDetail> TblBusDetails { get; set; }

    public virtual DbSet<TblPayment> TblPayments { get; set; }

    public virtual DbSet<TblRoute> TblRoutes { get; set; }

    public virtual DbSet<TblSchedule> TblSchedules { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Book__3214EC27DB02C840");

            entity.ToTable("Tbl_Book");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Phoneno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ScheduleId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBusDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_BusD__3214EC0710818D4B");

            entity.ToTable("Tbl_BusDetail");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.BusName).HasMaxLength(100);
            entity.Property(e => e.BusNo).HasMaxLength(10);
            entity.Property(e => e.BusType).HasMaxLength(10);
        });

        modelBuilder.Entity<TblPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Paym__3214EC070A51E68F");

            entity.ToTable("Tbl_Payment");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.BookId).HasMaxLength(100);
            entity.Property(e => e.CardNo).HasMaxLength(50);
            entity.Property(e => e.Payment).HasMaxLength(50);
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblRoute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Rout__3214EC071BD6EB66");

            entity.ToTable("Tbl_Route");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.Origin).HasMaxLength(100);
            entity.Property(e => e.RouteName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Sche__3214EC07E61E4650");

            entity.ToTable("Tbl_Schedule");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.ArrivalTime).HasMaxLength(20);
            entity.Property(e => e.BusId).HasMaxLength(100);
            entity.Property(e => e.DepartureTime).HasMaxLength(20);
            entity.Property(e => e.RouteId).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
