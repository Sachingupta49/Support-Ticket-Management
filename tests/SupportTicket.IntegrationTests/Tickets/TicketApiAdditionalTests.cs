using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SupportTicket.IntegrationTests.Infrastructure;

namespace SupportTicket.IntegrationTests.Tickets;

public class TicketApiAdditionalTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TicketApiAdditionalTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateTicket_NotFound_Returns404()
    {
        var response = await _client.PutAsJsonAsync("/api/tickets/9999", new
        {
            title = "Title",
            description = "Description",
            priority = "Low",
            assigneeId = 1
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTickets_InvalidStatus_Returns400()
    {
        var response = await _client.GetAsync("/api/tickets?status=InvalidStatus");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeStatus_NotFound_Returns404()
    {
        var response = await _client.PatchAsJsonAsync("/api/tickets/9999/status", new { status = "InProgress" });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangeStatus_InvalidStatusValue_Returns400()
    {
        var response = await _client.PatchAsJsonAsync("/api/tickets/1/status", new { status = "NotAStatus" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
