using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EduAnalytics.QueryService.IntegrationTests;

public class AnalyticsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AnalyticsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Query_WithValidQuestion_ReturnsOkResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new { Question = "朝阳区2024年上学期八年级数学的平均分" };

        // Act
        var response = await client.PostAsJsonAsync("/api/analytics/query", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<QueryResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Query_WithEmptyQuestion_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new { Question = "" };

        // Act
        var response = await client.PostAsJsonAsync("/api/analytics/query", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DrillDown_WithWhyQuestion_ReturnsDrillDownPath()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new { Question = "为什么朝阳区的数学平均分低于全区平均分" };

        // Act
        var response = await client.PostAsJsonAsync("/api/analytics/drilldown", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<DrillDownResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.DrillDownPath.Should().HaveCount(3);
        result.RootCause.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Health_ReturnsHealthy()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/analytics/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
        Assert.Equal("healthy", result.GetProperty("status").GetString());
    }
}