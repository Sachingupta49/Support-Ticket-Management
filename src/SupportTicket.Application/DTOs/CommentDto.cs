namespace SupportTicket.Application.DTOs;

public record CommentDto(
    int Id,
    int TicketId,
    int AuthorId,
    string AuthorName,
    string Body,
    DateTime CreatedAt);
