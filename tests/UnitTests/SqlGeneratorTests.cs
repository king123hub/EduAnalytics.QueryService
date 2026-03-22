using Xunit;
using FluentAssertions;
using EduAnalytics.QueryService.Application.SqlGenerator;
using EduAnalytics.QueryService.Application.IntentParser.Models;
using EduAnalytics.QueryService.Domain.Dimensions;

namespace EduAnalytics.QueryService.UnitTests;

public class SqlGeneratorTests
{
    private readonly ISqlGenerator _sqlGenerator;

    public SqlGeneratorTests()
    {
        _sqlGenerator = new EduSqlGenerator();
    }

    [Fact]
    public void GenerateSql_WithSimpleScoreQuery_ReturnsCorrectSql()
    {
        // Arrange
        var intent = new ParsedIntent
        {
            AnalysisType = AnalysisType.Score,
            Metrics = new List<string> { "AvgScore" },
            Space = new SpaceDimension { District = "朝阳区", Grade = 8 },
            Time = new TimeDimension { AcademicYear = 2024, Semester = "上学期" },
            Content = new ContentDimension { Subject = "数学" }
        };

        // Act
        var sql = _sqlGenerator.GenerateSql(intent);

        // Assert
        sql.Should().Contain("SELECT AVG(Score)");
        sql.Should().Contain("District = '朝阳区'");
        sql.Should().Contain("Grade = 8");
        sql.Should().Contain("AcademicYear = 2024");
        sql.Should().Contain("Semester = '上学期'");
        sql.Should().Contain("Subject = '数学'");
    }

    [Fact]
    public void GenerateSql_WithCorrectRateQuery_ReturnsCorrectSql()
    {
        // Arrange
        var intent = new ParsedIntent
        {
            AnalysisType = AnalysisType.Mastery,
            Metrics = new List<string> { "CorrectRate" },
            Space = new SpaceDimension { School = "朝阳一小", Class = "3班" },
            Content = new ContentDimension { Chapter = "二次函数" }
        };

        // Act
        var sql = _sqlGenerator.GenerateSql(intent);

        // Assert
        sql.Should().Contain("CorrectRate");
        sql.Should().Contain("School = '朝阳一小'");
        sql.Should().Contain("Class = '3班'");
        sql.Should().Contain("Chapter = '二次函数'");
    }

    [Fact]
    public void GenerateSql_WithMultipleMetrics_IncludesAllMetrics()
    {
        // Arrange
        var intent = new ParsedIntent
        {
            Metrics = new List<string> { "AvgScore", "CorrectRate", "AnswerCount" }
        };

        // Act
        var sql = _sqlGenerator.GenerateSql(intent);

        // Assert
        sql.Should().Contain("AVG(Score)");
        sql.Should().Contain("CorrectRate");
        sql.Should().Contain("COUNT(*)");
    }

    [Fact]
    public void GenerateSql_WithEmptyDimensions_NoWhereClause()
    {
        // Arrange
        var intent = new ParsedIntent
        {
            Metrics = new List<string> { "AnswerCount" }
        };

        // Act
        var sql = _sqlGenerator.GenerateSql(intent);

        // Assert
        sql.Should().NotContain("WHERE");
        sql.Should().Be("SELECT COUNT(*) FROM FactStudentAnswer WITH (NOLOCK) ");
    }
}