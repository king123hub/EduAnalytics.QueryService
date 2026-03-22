using Xunit;
using FluentAssertions;
using EduAnalytics.QueryService.Application.IntentParser;

namespace EduAnalytics.QueryService.UnitTests;

public class IntentParserTests
{
    private readonly IIntentParser _parser;

    public IntentParserTests()
    {
        _parser = new MockLLMParser();
    }

    [Fact]
    public async Task ParseAsync_WithScoreQuestion_ReturnsScoreIntent()
    {
        // Arrange
        var question = "朝阳区2024年上学期八年级数学的平均分是多少";

        // Act
        var intent = await _parser.ParseAsync(question);

        // Assert
        intent.AnalysisType.Should().Be(AnalysisType.Score);
        intent.Metrics.Should().Contain("AvgScore");
        intent.Space.District.Should().Be("朝阳区");
        intent.Space.Grade.Should().Be(8);
        intent.Time.AcademicYear.Should().Be(2024);
        intent.Time.Semester.Should().Be("上学期");
        intent.Content.Subject.Should().Be("数学");
    }

    [Fact]
    public async Task ParseAsync_WithWhyQuestion_IncludesDrillDown()
    {
        // Arrange
        var question = "为什么朝阳区的数学平均分低于全区平均分";

        // Act
        var intent = await _parser.ParseAsync(question);

        // Assert
        intent.AnalysisType.Should().Be(AnalysisType.Compare);
        intent.DrillDown.Should().NotBeNull();
        intent.DrillDown!.Dimension.Should().Be("subject");
    }

    [Fact]
    public async Task ParseAsync_WithCorrectRateQuestion_ReturnsMasteryIntent()
    {
        // Arrange
        var question = "朝阳一小3班二次函数章节的正确率";

        // Act
        var intent = await _parser.ParseAsync(question);

        // Assert
        intent.AnalysisType.Should().Be(AnalysisType.Mastery);
        intent.Metrics.Should().Contain("CorrectRate");
        intent.Space.School.Should().Be("朝阳一小");
        intent.Space.Class.Should().Be("3班");
        intent.Content.Chapter.Should().Be("二次函数");
    }
}