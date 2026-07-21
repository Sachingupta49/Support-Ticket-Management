using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Mappings;

namespace SupportTicket.Application.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);

        return users.Select(u => u.ToDto()).ToList();
    }
}
