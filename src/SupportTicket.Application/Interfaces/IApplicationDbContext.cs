using Microsoft.EntityFrameworkCore;
using SupportTicket.Domain.Entities;

namespace SupportTicket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<Comment> Comments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
