using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogDetail> BlogDetails { get; set; }

    public virtual DbSet<BlogHeader> BlogHeaders { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblIncompatibleFood> TblIncompatibleFoods { get; set; }

    public virtual DbSet<TblStudent> TblStudents { get; set; }

    public virtual DbSet<TblTransactionRecord> TblTransactionRecords { get; set; }

    public virtual DbSet<TblWallet> TblWallets { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogDetail>(entity =>
        {
            entity.HasKey(e => e.BlogDetailId).HasName("PK__BlogDeta__2383E83E627D1F56");

            entity.ToTable("BlogDetail");

            entity.Property(e => e.BlogDetailId).ValueGeneratedNever();

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogDetails)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_BlogHeader");
        });

        modelBuilder.Entity<BlogHeader>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__BlogHead__54379E30E6916A26");

            entity.ToTable("BlogHeader");

            entity.Property(e => e.BlogId).ValueGeneratedNever();
            entity.Property(e => e.BlogTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Results__3214EC074CACAC63");

            entity.HasIndex(e => e.StudentId, "IX_Results_StudentId");

            entity.Property(e => e.Grade).HasMaxLength(5);

            entity.HasOne(d => d.Student).WithMany(p => p.Results)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Results_Student");

            entity.HasOne(d => d.Subject).WithMany(p => p.Results)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Results_Subject");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC074C73759C");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D1053415DF542E").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subjects__3214EC07FC3A9ABC");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("Tbl_Account");

            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblIncompatibleFood>(entity =>
        {
            entity.ToTable("Tbl_IncompatibleFood");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.FoodA).HasMaxLength(50);
            entity.Property(e => e.FoodB).HasMaxLength(50);
        });

        modelBuilder.Entity<TblStudent>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.ToTable("Tbl_Student");

            entity.Property(e => e.ClassNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTransactionRecord>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Tbl_Tran__55433A6B82D2D935");

            entity.ToTable("Tbl_TransactionRecord");

            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FromMobileNo).HasMaxLength(20);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ToMobileNo).HasMaxLength(20);
            entity.Property(e => e.TxnId).HasMaxLength(100);
        });

        modelBuilder.Entity<TblWallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Tbl_Wall__84D4F92E398F2432");

            entity.ToTable("Tbl_Wallet");

            entity.Property(e => e.WalletId)
                .HasMaxLength(100)
                .HasColumnName("WalletID");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.MobileNo).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
