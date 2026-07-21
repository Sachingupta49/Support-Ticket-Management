using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;

namespace SupportTicket.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        Seed.DatabaseSeed.SeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
