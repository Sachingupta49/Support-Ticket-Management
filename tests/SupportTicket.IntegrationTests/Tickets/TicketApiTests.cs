using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.IntegrationTests.Infrastructure;

namespace SupportTicket.IntegrationTests.Tickets;

public class TicketApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public TicketApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTickets_ReturnsSeededTickets()
    {
        var response = await _client.GetAsync("/api/tickets");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDto>>(JsonOptions);
        tickets.Should().NotBeNull();
        tickets!.Should().HaveCountGreaterOrEqualTo(5);
    }

    [Fact]
    public async Task GetTicketById_ExistingTicket_ReturnsTicket()
    {
        var response = await _client.GetAsync("/api/tickets/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket.Should().NotBeNull();
        ticket!.Id.Should().Be(1);
        ticket.Status.Should().Be("Open");
        ticket.AssigneeName.Should().Be("Alice Agent");
    }

    [Fact]
    public async Task GetTicketById_NotFound_Returns404()
    {
        var response = await _client.GetAsync("/api/tickets/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTicket_ValidRequest_Returns201WithOpenStatus()
    {
        var request = new CreateTicketRequest
        {
            Title = "Integration test ticket",
            Description = "Created by integration test",
            Priority = "Medium",
            AssigneeId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        ticket.Should().NotBeNull();
        ticket!.Title.Should().Be("Integration test ticket");
        ticket.Status.Should().Be("Open");
    }

    [Fact]
    public async Task CreateTicket_MissingTitle_Returns400()
    {
        var request = new CreateTicketRequest
        {
            Title = "",
            Description = "Description",
            Priority = "Medium",
            AssigneeId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Title");
    }

    [Fact]
    public async Task CreateTicket_InvalidPriority_Returns400()
    {
        var request = new CreateTicketRequest
        {
            Title = "Title",
            Description = "Description",
            Priority = "Urgent",
            AssigneeId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Priority");
    }

    [Fact]
    public async Task UpdateTicket_ValidRequest_Returns200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Ticket to update",
            Description = "Original description",
            Priority = "Low",
            AssigneeId = 1
        });

        var created = await createResponse.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);

        var updateRequest = new UpdateTicketRequest
        {
            Title = "Updated title",
            Description = "Updated description",
            Priority = "High",
            AssigneeId = 2
        };

        var response = await _client.PutAsJsonAsync($"/api/tickets/{created!.Id}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<TicketDto>(JsonOptions);
        updated!.Title.Should().Be("Updated title");
        updated.Priority.Should().Be("High");
        updated.AssigneeId.Should().Be(2);
    }

    [Fact]
    public async Task GetTickets_WithSearch_ReturnsMatchingTickets()
    {
        var response = await _client.GetAsync("/api/tickets?search=login");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDto>>(JsonOptions);
        tickets.Should().NotBeNull();
        tickets!.Should().OnlyContain(t =>
            t.Title.Contains("login", StringComparison.OrdinalIgnoreCase) ||
            t.Description.Contains("login", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetTickets_WithStatusFilter_ReturnsOnlyMatchingStatus()
    {
        var response = await _client.GetAsync("/api/tickets?status=Open");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDto>>(JsonOptions);
        tickets.Should().NotBeNull();
        tickets!.Should().OnlyContain(t => t.Status == "Open");
    }

    [Fact]
    public async Task GetTickets_WithSearchAndStatus_ReturnsCombinedResults()
    {
        var response = await _client.GetAsync("/api/tickets?search=printer&status=InProgress");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDto>>(JsonOptions);
        tickets.Should().NotBeNull();
        tickets!.Should().HaveCount(1);
        tickets![0].Title.Should().Contain("Printer");
        tickets[0].Status.Should().Be("InProgress");
    }
}
