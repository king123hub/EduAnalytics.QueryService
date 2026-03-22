using EduAnalytics.QueryService.Application.IntentParser.Models;

namespace EduAnalytics.QueryService.Application.IntentParser;

/// <summary>
/// 意图解析器接口
/// </summary>
public interface IIntentParser
{
    /// <summary>
    /// 解析自然语言问题为结构化意图
    /// </summary>
    Task<ParsedIntent> ParseAsync(string question);
}