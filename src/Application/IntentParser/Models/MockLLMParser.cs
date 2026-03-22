using EduAnalytics.QueryService.Application.IntentParser.Models;
using EduAnalytics.QueryService.Domain.Dimensions;

namespace EduAnalytics.QueryService.Application.IntentParser;

/// <summary>
/// 模拟大模型解析器
/// 真实场景会调用LLM API，这里用规则模拟返回结构化JSON
/// </summary>
public class MockLLMParser : IIntentParser
{
    public Task<ParsedIntent> ParseAsync(string question)
    {
        var intent = new ParsedIntent
        {
            OriginalQuestion = question
        };

        // 模拟解析逻辑：根据关键词匹配
        if (question.Contains("平均分") || question.Contains("成绩"))
        {
            intent.AnalysisType = AnalysisType.Score;
            intent.Metrics.Add("AvgScore");
        }
        else if (question.Contains("正确率"))
        {
            intent.AnalysisType = AnalysisType.Mastery;
            intent.Metrics.Add("CorrectRate");
        }
        else if (question.Contains("为什么"))
        {
            intent.AnalysisType = AnalysisType.Compare;
            intent.DrillDown = new DrillDownInfo
            {
                Dimension = "subject",
                Baseline = "全区平均分"
            };
        }

        // 解析空间维度
        if (question.Contains("朝阳区"))
        {
            intent.Space.District = "朝阳区";
        }
        if (question.Contains("八年级"))
        {
            intent.Space.Grade = 8;
        }
        if (question.Contains("三班") || question.Contains("3班"))
        {
            intent.Space.Class = "3班";
        }

        // 解析时间维度
        if (question.Contains("2024"))
        {
            intent.Time.AcademicYear = 2024;
        }
        if (question.Contains("上学期"))
        {
            intent.Time.Semester = "上学期";
        }

        // 解析内容维度
        if (question.Contains("数学"))
        {
            intent.Content.Subject = "数学";
        }
        if (question.Contains("二次函数"))
        {
            intent.Content.Chapter = "二次函数";
        }

        return Task.FromResult(intent);
    }
}