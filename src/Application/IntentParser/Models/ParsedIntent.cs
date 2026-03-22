using EduAnalytics.QueryService.Domain.Dimensions;

namespace EduAnalytics.QueryService.Application.IntentParser.Models;

/// <summary>
/// 大模型解析后的结构化意图
/// </summary>
public class ParsedIntent
{
    /// <summary>
    /// 分析类型
    /// </summary>
    public AnalysisType AnalysisType { get; set; }
    
    /// <summary>
    /// 空间维度
    /// </summary>
    public SpaceDimension Space { get; set; } = new();
    
    /// <summary>
    /// 时间维度
    /// </summary>
    public TimeDimension Time { get; set; } = new();
    
    /// <summary>
    /// 内容维度
    /// </summary>
    public ContentDimension Content { get; set; } = new();
    
    /// <summary>
    /// 统计指标
    /// </summary>
    public List<string> Metrics { get; set; } = new();
    
    /// <summary>
    /// 筛选条件
    /// </summary>
    public Dictionary<string, object> Filters { get; set; } = new();
    
    /// <summary>
    /// 下钻信息（问"为什么"时使用）
    /// </summary>
    public DrillDownInfo? DrillDown { get; set; }
    
    /// <summary>
    /// 原始问题
    /// </summary>
    public string OriginalQuestion { get; set; } = string.Empty;
}

/// <summary>
/// 下钻信息
/// </summary>
public class DrillDownInfo
{
    /// <summary>
    /// 下钻维度（subject/chapter/question）
    /// </summary>
    public string Dimension { get; set; } = string.Empty;
    
    /// <summary>
    /// 当前维度值
    /// </summary>
    public string CurrentValue { get; set; } = string.Empty;
    
    /// <summary>
    /// 对比基准
    /// </summary>
    public string Baseline { get; set; } = string.Empty;
}