using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Interfaces;

public interface ITicketService
{
    Task<IReadOnlyList<TicketDto>> GetTicketsAsync(string? search, TicketStatus? status, CancellationToken cancellationToken = default);
    Task<TicketDto> GetTicketByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TicketDto> CreateTicketAsync(CreateTicketRequest request, CancellationToken cancellationToken = default);
    Task<TicketDto> UpdateTicketAsync(int id, UpdateTicketRequest request, CancellationToken cancellationToken = default);
}
