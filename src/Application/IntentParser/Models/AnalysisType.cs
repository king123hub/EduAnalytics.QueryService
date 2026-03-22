namespace EduAnalytics.QueryService.Application.IntentParser.Models;

/// <summary>
/// 分析类型
/// </summary>
public enum AnalysisType
{
    Score,      // 成绩分析
    Mastery,    // 掌握情况
    Mistake,    // 错题分析
    Trend,      // 趋势分析
    Compare     // 对比分析
}