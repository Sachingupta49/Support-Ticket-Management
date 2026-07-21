using Microsoft.EntityFrameworkCore;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Infrastructure.Persistence.Seed;

public static class DatabaseSeed
{
    private static readonly DateTime SeedDate = new(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc);

    public static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Alice Agent", Email = "alice@support.com", CreatedAt = SeedDate },
            new User { Id = 2, Name = "Bob Agent", Email = "bob@support.com", CreatedAt = SeedDate },
            new User { Id = 3, Name = "Carol Manager", Email = "carol@support.com", CreatedAt = SeedDate });

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id = 1,
                Title = "Cannot login to portal",
                Description = "User reports 401 error on login page after password reset.",
                Status = TicketStatus.Open,
                Priority = TicketPriority.High,
                AssigneeId = 1,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new Ticket
            {
                Id = 2,
                Title = "Printer not working",
                Description = "Office printer on floor 3 is not responding to print jobs.",
                Status = TicketStatus.InProgress,
                Priority = TicketPriority.Medium,
                AssigneeId = 2,
                CreatedAt = SeedDate.AddHours(1),
                UpdatedAt = SeedDate.AddHours(2)
            },
            new Ticket
            {
                Id = 3,
                Title = "Email sync delay",
                Description = "Outlook emails are delayed by 15-20 minutes.",
                Status = TicketStatus.Resolved,
                Priority = TicketPriority.Low,
                AssigneeId = 1,
                CreatedAt = SeedDate.AddHours(3),
                UpdatedAt = SeedDate.AddHours(5)
            },
            new Ticket
            {
                Id = 4,
                Title = "VPN connection drops",
                Description = "VPN disconnects every 30 minutes for remote users.",
                Status = TicketStatus.Closed,
                Priority = TicketPriority.High,
                AssigneeId = 3,
                CreatedAt = SeedDate.AddDays(-1),
                UpdatedAt = SeedDate.AddHours(6)
            },
            new Ticket
            {
                Id = 5,
                Title = "Request new monitor",
                Description = "Employee requested a 27-inch monitor for home office setup.",
                Status = TicketStatus.Cancelled,
                Priority = TicketPriority.Low,
                AssigneeId = 2,
                CreatedAt = SeedDate.AddDays(-2),
                UpdatedAt = SeedDate.AddHours(1)
            });

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                TicketId = 1,
                AuthorId = 2,
                Body = "Checking authentication logs for this user account.",
                CreatedAt = SeedDate.AddMinutes(30)
            },
            new Comment
            {
                Id = 2,
                TicketId = 2,
                AuthorId = 2,
                Body = "Restarted the print spooler service. Monitoring.",
                CreatedAt = SeedDate.AddHours(2)
            },
            new Comment
            {
                Id = 3,
                TicketId = 3,
                AuthorId = 1,
                Body = "Mail queue was backed up. Cleared and sync is normal now.",
                CreatedAt = SeedDate.AddHours(5)
            });
    }
}
