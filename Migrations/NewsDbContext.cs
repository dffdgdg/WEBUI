using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Migrations;

public partial class NewsDbContext : DbContext
{
    public NewsDbContext()
    {
    }

    public NewsDbContext(DbContextOptions<NewsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NewsDB;Username=postgres;Password=qwaszxedc1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comments_pkey");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.News).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NewsId)
                .HasConstraintName("Comments_NewsId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Comments_UserId_fkey");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("News_pkey");

            entity.HasIndex(e => e.AuthorId, "IX_News_AuthorId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("News_AuthorId_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetRoles_pkey");

            entity.HasIndex(e => e.Name, "AspNetRoles_Name_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"AspNetRoles_Id_seq\"'::regclass)");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetUsers_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"AspNetUsers_Id_seq\"'::regclass)");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed).HasDefaultValue(false);
            entity.Property(e => e.Login).HasMaxLength(75);
            entity.Property(e => e.Midname).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
