using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.Common;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Mappings;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
using SupportTicket.Domain.Exceptions;

namespace SupportTicket.Application.Services;

public class TicketService : ITicketService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateTicketRequest> _createValidator;
    private readonly IValidator<UpdateTicketRequest> _updateValidator;
    private readonly IValidator<UpdateTicketStatusRequest> _statusValidator;
    private readonly IStatusTransitionValidator _statusTransitionValidator;

    public TicketService(
        IApplicationDbContext context,
        IValidator<CreateTicketRequest> createValidator,
        IValidator<UpdateTicketRequest> updateValidator,
        IValidator<UpdateTicketStatusRequest> statusValidator,
        IStatusTransitionValidator statusTransitionValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _statusValidator = statusValidator;
        _statusTransitionValidator = statusTransitionValidator;
    }

    public async Task<IReadOnlyList<TicketDto>> GetTicketsAsync(
        string? search,
        TicketStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tickets
            .Include(t => t.Assignee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(term) ||
                t.Description.ToLower().Contains(term));
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var tickets = await query
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync(cancellationToken);

        return tickets.Select(t => t.ToDto()).ToList();
    }

    public async Task<TicketDto> GetTicketByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Assignee)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException("Ticket", id);
        }

        return ticket.ToDto();
    }

    public async Task<TicketDto> CreateTicketAsync(CreateTicketRequest request, CancellationToken cancellationToken = default)
    {
        await ValidationHelper.ValidateAsync(_createValidator, request, cancellationToken);

        await EnsureUserExistsAsync(request.AssigneeId, cancellationToken);

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = EntityMappings.ParsePriority(request.Priority),
            AssigneeId = request.AssigneeId,
            Status = TicketStatus.Open,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        var created = await _context.Tickets
            .Include(t => t.Assignee)
            .FirstAsync(t => t.Id == ticket.Id, cancellationToken);

        return created.ToDto();
    }

    public async Task<TicketDto> UpdateTicketAsync(int id, UpdateTicketRequest request, CancellationToken cancellationToken = default)
    {
        await ValidationHelper.ValidateAsync(_updateValidator, request, cancellationToken);

        var ticket = await _context.Tickets
            .Include(t => t.Assignee)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException("Ticket", id);
        }

        await EnsureUserExistsAsync(request.AssigneeId, cancellationToken);

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();
        ticket.Priority = EntityMappings.ParsePriority(request.Priority);
        ticket.AssigneeId = request.AssigneeId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var updated = await _context.Tickets
            .Include(t => t.Assignee)
            .FirstAsync(t => t.Id == id, cancellationToken);

        return updated.ToDto();
    }

    public async Task<TicketDto> ChangeStatusAsync(
        int id,
        UpdateTicketStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        await ValidationHelper.ValidateAsync(_statusValidator, request, cancellationToken);

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException("Ticket", id);
        }

        var newStatus = EntityMappings.ParseStatus(request.Status)!.Value;
        _statusTransitionValidator.Validate(ticket.Status, newStatus);

        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var updated = await _context.Tickets
            .Include(t => t.Assignee)
            .FirstAsync(t => t.Id == id, cancellationToken);

        return updated.ToDto();
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
