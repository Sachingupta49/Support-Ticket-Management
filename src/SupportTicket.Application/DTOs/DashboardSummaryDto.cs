namespace SupportTicket.Application.DTOs;

public record DashboardSummaryDto(
    int TotalTickets,
    Dictionary<string, int> ByStatus,
    IReadOnlyList<TicketDto> RecentTickets);
