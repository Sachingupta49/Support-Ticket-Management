namespace SupportTicket.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int AuthorId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Ticket Ticket { get; set; } = null!;
    public User Author { get; set; } = null!;
}
