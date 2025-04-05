using System.Reflection;
using DotNetCore.Packages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Packages.Persistence.DbContexts;

public class EFBaseDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public EFBaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();

        base.OnModelCreating(modelBuilder);
    }
}