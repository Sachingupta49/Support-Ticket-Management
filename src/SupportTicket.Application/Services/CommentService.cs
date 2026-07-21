using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.Common;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Mappings;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Exceptions;

namespace SupportTicket.Application.Services;

public class CommentService : ICommentService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateCommentRequest> _createValidator;

    public CommentService(
        IApplicationDbContext context,
        IValidator<CreateCommentRequest> createValidator)
    {
        _context = context;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<CommentDto>> GetCommentsByTicketIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
    {
        await EnsureTicketExistsAsync(ticketId, cancellationToken);

        var comments = await _context.Comments
            .Include(c => c.Author)
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        return comments.Select(c => c.ToDto()).ToList();
    }

    public async Task<CommentDto> CreateCommentAsync(
        int ticketId,
        CreateCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        await ValidationHelper.ValidateAsync(_createValidator, request, cancellationToken);
        await EnsureTicketExistsAsync(ticketId, cancellationToken);
        await EnsureUserExistsAsync(request.AuthorId, cancellationToken);

        var comment = new Comment
        {
            TicketId = ticketId,
            AuthorId = request.AuthorId,
            Body = request.Body.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        var created = await _context.Comments
            .Include(c => c.Author)
            .FirstAsync(c => c.Id == comment.Id, cancellationToken);

        return created.ToDto();
    }

    private async Task EnsureTicketExistsAsync(int ticketId, CancellationToken cancellationToken)
    {
        var exists = await _context.Tickets.AnyAsync(t => t.Id == ticketId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException("Ticket", ticketId);
        }
    }

    private async Task EnsureUserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        var exists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException("User", userId);
        }
    }
}
