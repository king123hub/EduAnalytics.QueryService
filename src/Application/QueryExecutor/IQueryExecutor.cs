using EduAnalytics.QueryService.Domain.Dimensions;

namespace EduAnalytics.QueryService.Application.QueryExecutor;

/// <summary>
/// 查询执行器接口
/// </summary>
public interface IQueryExecutor
{
    /// <summary>
    /// 执行SQL查询并返回统计指标
    /// </summary>
    Task<StatisticMetrics> ExecuteAsync(string sql);
}