using Microsoft.AspNetCore.Mvc;
using EduAnalytics.QueryService.Application.IntentParser;
using EduAnalytics.QueryService.Application.IntentParser.Models;
using EduAnalytics.QueryService.Application.SqlGenerator;
using EduAnalytics.QueryService.Application.QueryExecutor;

namespace EduAnalytics.QueryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnalyticsController : ControllerBase
{
    private readonly IIntentParser _intentParser;
    private readonly ISqlGenerator _sqlGenerator;
    private readonly IQueryExecutor _queryExecutor;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IIntentParser intentParser,
        ISqlGenerator sqlGenerator,
        IQueryExecutor queryExecutor,
        ILogger<AnalyticsController> logger)
    {
        _intentParser = intentParser;
        _sqlGenerator = sqlGenerator;
        _queryExecutor = queryExecutor;
        _logger = logger;
    }

    /// <summary>
    /// 自然语言查询
    /// </summary>
    /// <param name="request">包含自然语言问题的请求</param>
    /// <returns>查询结果</returns>
    [HttpPost("query")]
    [ProducesResponseType(typeof(QueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Query([FromBody] QueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { error = "问题不能为空" });
        }

        try
        {
            _logger.LogInformation("收到查询请求: {Question}", request.Question);

            // 1. 解析意图
            var intent = await _intentParser.ParseAsync(request.Question);
            _logger.LogInformation("意图解析结果: {@Intent}", intent);

            // 2. 生成SQL
            var sql = _sqlGenerator.GenerateSql(intent);
            _logger.LogInformation("生成的SQL: {Sql}", sql);

            // 3. 执行查询
            var result = await _queryExecutor.ExecuteAsync(sql);

            // 4. 返回结果
            return Ok(new QueryResponse
            {
                Success = true,
                Data = result,
                Sql = sql,
                ParsedIntent = intent
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "查询处理失败");
            return StatusCode(500, new { error = "查询处理失败", detail = ex.Message });
        }
    }

    /// <summary>
    /// 智能下钻查询（问"为什么"）
    /// </summary>
    [HttpPost("drilldown")]
    [ProducesResponseType(typeof(DrillDownResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DrillDown([FromBody] QueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { error = "问题不能为空" });
        }

        // 模拟下钻路径
        var drillDownPath = new List<DrillDownLevel>
        {
            new() { Level = "学科", Value = "数学", AvgScore = 76.5 },
            new() { Level = "章节", Value = "二次函数", AvgScore = 68.2 },
            new() { Level = "试题", Value = "Q-1023", CorrectRate = 0.45 }
        };

        var response = new DrillDownResponse
        {
            Success = true,
            RootCause = "试题Q-1023（二次函数应用）正确率仅45%，拉低了整体平均分",
            DrillDownPath = drillDownPath
        };

        return Ok(response);
    }

    /// <summary>
    /// 健康检查
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}

/// <summary>
/// 查询请求
/// </summary>
public class QueryRequest
{
    public string Question { get; set; } = string.Empty;
}

/// <summary>
/// 查询响应
/// </summary>
public class QueryResponse
{
    public bool Success { get; set; }
    public object? Data { get; set; }
    public string? Sql { get; set; }
    public object? ParsedIntent { get; set; }
    public long ExecutionTimeMs { get; set; }
}

/// <summary>
/// 下钻响应
/// </summary>
public class DrillDownResponse
{
    public bool Success { get; set; }
    public string? RootCause { get; set; }
    public List<DrillDownLevel> DrillDownPath { get; set; } = new();
}

/// <summary>
/// 下钻层级
/// </summary>
public class DrillDownLevel
{
    public string Level { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double? AvgScore { get; set; }
    public double? CorrectRate { get; set; }
}