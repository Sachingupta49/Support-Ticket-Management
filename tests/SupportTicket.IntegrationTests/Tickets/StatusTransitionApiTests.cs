using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.IntegrationTests.Infrastructure;

namespace SupportTicket.IntegrationTests.Tickets;

public class StatusTransitionApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public StatusTransitionApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ChangeStatus_OpenToInProgress_Succeeds()
    {
        var ticketId = await CreateTicketAsync("Open to InProgress test");

        var response = await PatchStatus(ticketId, "InProgress");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket!.Status.Should().Be("InProgress");
    }

    [Fact]
    public async Task ChangeStatus_InProgressToResolved_Succeeds()
    {
        var ticketId = await CreateTicketAsync("InProgress to Resolved test");
        await PatchStatus(ticketId, "InProgress");

        var response = await PatchStatus(ticketId, "Resolved");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket!.Status.Should().Be("Resolved");
    }

    [Fact]
    public async Task ChangeStatus_ResolvedToClosed_Succeeds()
    {
        var ticketId = await CreateTicketAsync("Resolved to Closed test");
        await PatchStatus(ticketId, "InProgress");
        await PatchStatus(ticketId, "Resolved");

        var response = await PatchStatus(ticketId, "Closed");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket!.Status.Should().Be("Closed");
    }

    [Fact]
    public async Task ChangeStatus_OpenToCancelled_Succeeds()
    {
        var ticketId = await CreateTicketAsync("Open to Cancelled test");

        var response = await PatchStatus(ticketId, "Cancelled");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket!.Status.Should().Be("Cancelled");
    }

    [Fact]
    public async Task ChangeStatus_InProgressToCancelled_Succeeds()
    {
        var ticketId = await CreateTicketAsync("InProgress to Cancelled test");
        await PatchStatus(ticketId, "InProgress");

        var response = await PatchStatus(ticketId, "Cancelled");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket!.Status.Should().Be("Cancelled");
    }

    [Fact]
    public async Task ChangeStatus_OpenToResolved_Returns400()
    {
        var ticketId = await CreateTicketAsync("Invalid Open to Resolved test");

        var response = await PatchStatus(ticketId, "Resolved");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Cannot transition from Open to Resolved");
    }

    [Fact]
    public async Task ChangeStatus_ResolvedToOpen_Returns400()
    {
        var response = await PatchStatus(3, "Open");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Cannot transition from Resolved to Open");
    }

    [Fact]
    public async Task ChangeStatus_ClosedToInProgress_Returns400()
    {
        var response = await PatchStatus(4, "InProgress");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Cannot transition from Closed to InProgress");
    }

    [Fact]
    public async Task ChangeStatus_CancelledToOpen_Returns400()
    {
        var response = await PatchStatus(5, "Open");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Cannot transition from Cancelled to Open");
    }

    private async Task<int> CreateTicketAsync(string title)
    {
        var response = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = title,
            Description = "Status transition test ticket",
            Priority = "Medium",
            AssigneeId = 1
        });

        response.EnsureSuccessStatusCode();

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        return ticket!.Id;
    }

    private async Task<HttpResponseMessage> PatchStatus(int ticketId, string status)
    {
        return await _client.PatchAsJsonAsync(
            $"/api/tickets/{ticketId}/status",
            new UpdateTicketStatusRequest { Status = status });
    }
}
