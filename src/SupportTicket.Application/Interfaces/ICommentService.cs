using SupportTicket.Application.DTOs;

namespace SupportTicket.Application.Interfaces;

public interface ICommentService
{
    Task<IReadOnlyList<CommentDto>> GetCommentsByTicketIdAsync(int ticketId, CancellationToken cancellationToken = default);
    Task<CommentDto> CreateCommentAsync(int ticketId, CreateCommentRequest request, CancellationToken cancellationToken = default);
}
