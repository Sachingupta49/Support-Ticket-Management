using SupportTicket.Application.DTOs;

namespace SupportTicket.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}
