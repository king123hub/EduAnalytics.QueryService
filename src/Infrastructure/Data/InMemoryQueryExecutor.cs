using EduAnalytics.QueryService.Application.QueryExecutor;
using EduAnalytics.QueryService.Domain.Dimensions;
using EduAnalytics.QueryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduAnalytics.QueryService.Infrastructure.Data;

/// <summary>
/// 内存查询执行器（模拟SQL执行，返回Mock数据）
/// </summary>
public class InMemoryQueryExecutor : IQueryExecutor
{
    private readonly EduDbContext _context;
    private readonly ILogger<InMemoryQueryExecutor> _logger;

    public InMemoryQueryExecutor(EduDbContext context, ILogger<InMemoryQueryExecutor> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StatisticMetrics> ExecuteAsync(string sql)
    {
        var sw = Stopwatch.StartNew();
        
        _logger.LogInformation("执行SQL: {Sql}", sql);
        
        // 模拟SQL执行：从内存数据中计算统计指标
        var metrics = new StatisticMetrics();
        
        // 解析SQL中的条件（简化版）
        if (sql.Contains("AVG(Score)"))
        {
            metrics.AvgScore = await _context.StudentAnswers.AverageAsync(a => a.Score);
        }
        
        if (sql.Contains("CorrectRate"))
        {
            var total = await _context.StudentAnswers.CountAsync();
            var correct = await _context.StudentAnswers.CountAsync(a => a.IsCorrect);
            metrics.CorrectRate = total > 0 ? (double)correct / total : 0;
        }
        
        if (sql.Contains("COUNT(*)"))
        {
            metrics.AnswerCount = await _context.StudentAnswers.CountAsync();
        }
        
        sw.Stop();
        _logger.LogInformation("查询执行完成，耗时: {ElapsedMs}ms", sw.ElapsedMilliseconds);
        
        return metrics;
    }
}