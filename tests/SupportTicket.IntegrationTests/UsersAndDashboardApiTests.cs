using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.IntegrationTests.Infrastructure;

namespace SupportTicket.IntegrationTests;

public class UsersAndDashboardApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public UsersAndDashboardApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsSeededUsers()
    {
        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>(JsonOptions);
        users.Should().NotBeNull();
        users!.Should().HaveCountGreaterOrEqualTo(3);
        users.Should().Contain(u => u.Name == "Alice Agent");
    }

    [Fact]
    public async Task GetDashboardSummary_ReturnsCountsByStatus()
    {
        var response = await _client.GetAsync("/api/dashboard/summary");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var summary = await response.Content.ReadFromJsonAsync<DashboardSummaryDto>(JsonOptions);
        summary.Should().NotBeNull();
        summary!.TotalTickets.Should().BeGreaterThan(0);
        summary.ByStatus.Should().ContainKey("Open");
        summary.RecentTickets.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetHealth_ReturnsHealthy()
    {
        var response = await _client.GetAsync("/api/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("healthy");
    }
}
