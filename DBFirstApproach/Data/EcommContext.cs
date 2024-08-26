using System;
using System.Collections.Generic;
using DBFirstApproach.Models;
using Microsoft.EntityFrameworkCore;

namespace DBFirstApproach.Data;

public partial class EcommContext : DbContext
{
    public EcommContext()
    {
    }

    public EcommContext(DbContextOptions<EcommContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Pid);

            entity.ToTable("carts");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Pid);

            entity.ToTable("products");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
