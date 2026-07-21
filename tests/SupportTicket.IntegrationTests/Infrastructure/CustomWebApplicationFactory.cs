using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
using SupportTicket.Infrastructure.Persistence;

namespace SupportTicket.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _databaseName = $"IntegrationTests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(ApplicationDbContext) ||
                    d.ServiceType == typeof(IApplicationDbContext))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        SeedTestData(context);
    }

    public new Task DisposeAsync() => Task.CompletedTask;

    private static void SeedTestData(ApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        var seedDate = new DateTime(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc);

        context.Users.AddRange(
            new User { Id = 1, Name = "Alice Agent", Email = "alice@support.com", CreatedAt = seedDate },
            new User { Id = 2, Name = "Bob Agent", Email = "bob@support.com", CreatedAt = seedDate },
            new User { Id = 3, Name = "Carol Manager", Email = "carol@support.com", CreatedAt = seedDate });

        context.Tickets.AddRange(
            new Ticket
            {
                Id = 1,
                Title = "Cannot login to portal",
                Description = "User reports 401 error on login page.",
                Status = TicketStatus.Open,
                Priority = TicketPriority.High,
                AssigneeId = 1,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Ticket
            {
                Id = 2,
                Title = "Printer not working",
                Description = "Office printer is not responding.",
                Status = TicketStatus.InProgress,
                Priority = TicketPriority.Medium,
                AssigneeId = 2,
                CreatedAt = seedDate.AddHours(1),
                UpdatedAt = seedDate.AddHours(2)
            },
            new Ticket
            {
                Id = 3,
                Title = "Email sync delay",
                Description = "Outlook emails are delayed.",
                Status = TicketStatus.Resolved,
                Priority = TicketPriority.Low,
                AssigneeId = 1,
                CreatedAt = seedDate.AddHours(3),
                UpdatedAt = seedDate.AddHours(5)
            },
            new Ticket
            {
                Id = 4,
                Title = "VPN connection drops",
                Description = "VPN disconnects every 30 minutes.",
                Status = TicketStatus.Closed,
                Priority = TicketPriority.High,
                AssigneeId = 3,
                CreatedAt = seedDate.AddDays(-1),
                UpdatedAt = seedDate.AddHours(6)
            },
            new Ticket
            {
                Id = 5,
                Title = "Request new monitor",
                Description = "Employee requested a monitor.",
                Status = TicketStatus.Cancelled,
                Priority = TicketPriority.Low,
                AssigneeId = 2,
                CreatedAt = seedDate.AddDays(-2),
                UpdatedAt = seedDate.AddHours(1)
            });

        context.Comments.AddRange(
            new Comment
            {
                Id = 1,
                TicketId = 1,
                AuthorId = 2,
                Body = "Checking authentication logs.",
                CreatedAt = seedDate.AddMinutes(30)
            },
            new Comment
            {
                Id = 2,
                TicketId = 2,
                AuthorId = 2,
                Body = "Restarted print spooler.",
                CreatedAt = seedDate.AddHours(2)
            });

        context.SaveChanges();
    }
}
