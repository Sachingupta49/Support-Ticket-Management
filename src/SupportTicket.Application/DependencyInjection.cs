using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Services;
using SupportTicket.Application.Validators;

namespace SupportTicket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDashboardService, DashboardService>();

        services.AddValidatorsFromAssemblyContaining<CreateTicketRequestValidator>();

        return services;
    }
}
