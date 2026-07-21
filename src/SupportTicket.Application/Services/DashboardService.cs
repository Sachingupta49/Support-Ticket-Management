using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Mappings;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IApplicationDbContext _context;

    public DashboardService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var tickets = await _context.Tickets
            .Include(t => t.Assignee)
            .ToListAsync(cancellationToken);

        var byStatus = Enum.GetValues<TicketStatus>()
            .ToDictionary(
                status => status.ToString(),
                status => tickets.Count(t => t.Status == status));

        var recentTickets = tickets
            .OrderByDescending(t => t.UpdatedAt)
            .Take(5)
            .Select(t => t.ToDto())
            .ToList();

        return new DashboardSummaryDto(tickets.Count, byStatus, recentTickets);
    }
}
