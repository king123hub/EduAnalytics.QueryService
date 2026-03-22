using EduAnalytics.QueryService.Application.IntentParser.Models;

namespace EduAnalytics.QueryService.Application.SqlGenerator;

/// <summary>
/// SQL生成器接口
/// </summary>
public interface ISqlGenerator
{
    /// <summary>
    /// 根据解析的意图生成SQL
    /// </summary>
    string GenerateSql(ParsedIntent intent);
}