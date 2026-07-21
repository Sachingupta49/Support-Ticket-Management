namespace SupportTicket.Application.DTOs;

public class CreateCommentRequest
{
    public int AuthorId { get; set; }
    public string Body { get; set; } = string.Empty;
}
