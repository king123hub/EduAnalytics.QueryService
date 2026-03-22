namespace EduAnalytics.QueryService.Application.SqlGenerator;

/// <summary>
/// 维度字段映射器（业务字段 -> 数据库字段）
/// </summary>
public static class DimensionMapper
{
    private static readonly Dictionary<string, string> SpaceMapping = new()
    {
        ["City"] = "City",
        ["District"] = "District",
        ["School"] = "School",
        ["Grade"] = "Grade",
        ["Class"] = "Class",
        ["StudentId"] = "StudentId"
    };

    private static readonly Dictionary<string, string> TimeMapping = new()
    {
        ["AcademicYear"] = "AcademicYear",
        ["Semester"] = "Semester",
        ["Month"] = "Month",
        ["Week"] = "Week",
        ["Date"] = "AnswerDate",
        ["RecentDays"] = "AnswerTime"
    };

    private static readonly Dictionary<string, string> ContentMapping = new()
    {
        ["Subject"] = "Subject",
        ["Chapter"] = "Chapter",
        ["KnowledgePoint"] = "KnowledgePoint",
        ["Paper"] = "Paper",
        ["Question"] = "QuestionId"
    };

    private static readonly Dictionary<string, string> MetricsMapping = new()
    {
        ["AvgScore"] = "AVG(Score)",
        ["CorrectRate"] = "SUM(CASE WHEN IsCorrect=1 THEN 1.0 ELSE 0 END)/COUNT(*)",
        ["ErrorRate"] = "SUM(CASE WHEN IsCorrect=0 THEN 1.0 ELSE 0 END)/COUNT(*)",
        ["AnswerCount"] = "COUNT(*)"
    };

    /// <summary>
    /// 获取指标SQL片段
    /// </summary>
    public static string GetMetricSql(string metric)
    {
        return MetricsMapping.TryGetValue(metric, out var sql) ? sql : metric;
    }

    /// <summary>
    /// 获取维度字段名
    /// </summary>
    public static string GetSpaceField(string dimension)
    {
        return SpaceMapping.TryGetValue(dimension, out var field) ? field : dimension;
    }

    /// <summary>
    /// 获取时间字段名
    /// </summary>
    public static string GetTimeField(string dimension)
    {
        return TimeMapping.TryGetValue(dimension, out var field) ? field : dimension;
    }

    /// <summary>
    /// 获取内容字段名
    /// </summary>
    public static string GetContentField(string dimension)
    {
        return ContentMapping.TryGetValue(dimension, out var field) ? field : dimension;
    }
}