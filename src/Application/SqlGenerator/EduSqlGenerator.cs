using System.Text;
using EduAnalytics.QueryService.Application.IntentParser.Models;
using EduAnalytics.QueryService.Domain.Dimensions;

namespace EduAnalytics.QueryService.Application.SqlGenerator;

/// <summary>
/// 教育领域SQL生成器
/// </summary>
public class EduSqlGenerator : ISqlGenerator
{
    public string GenerateSql(ParsedIntent intent)
    {
        var sql = new StringBuilder();
        
        // SELECT 子句
        sql.Append("SELECT ");
        
        // 添加统计指标
        var metrics = intent.Metrics.Select(DimensionMapper.GetMetricSql);
        sql.Append(string.Join(", ", metrics));
        
        // FROM 子句
        sql.Append(" FROM FactStudentAnswer WITH (NOLOCK) ");
        
        // WHERE 子句
        var conditions = new List<string>();
        
        // 空间维度条件
        AddSpaceConditions(conditions, intent.Space);
        
        // 时间维度条件
        AddTimeConditions(conditions, intent.Time);
        
        // 内容维度条件
        AddContentConditions(conditions, intent.Content);
        
        // 自定义筛选条件
        foreach (var filter in intent.Filters)
        {
            conditions.Add($"{filter.Key} = '{filter.Value}'");
        }
        
        if (conditions.Any())
        {
            sql.Append("WHERE ");
            sql.Append(string.Join(" AND ", conditions));
        }
        
        // GROUP BY 子句（如果有维度分组）
        var groupBys = new List<string>();
        if (!string.IsNullOrEmpty(intent.Space.School))
            groupBys.Add("School");
        if (intent.Space.Grade.HasValue)
            groupBys.Add("Grade");
        if (!string.IsNullOrEmpty(intent.Content.Subject))
            groupBys.Add("Subject");
            
        if (groupBys.Any())
        {
            sql.Append(" GROUP BY ");
            sql.Append(string.Join(", ", groupBys));
        }
        
        // ORDER BY 子句（默认按指标降序）
        if (intent.Metrics.Contains("AvgScore"))
        {
            sql.Append(" ORDER BY AvgScore DESC");
        }
        
        return sql.ToString();
    }
    
    private void AddSpaceConditions(List<string> conditions, SpaceDimension space)
    {
        if (!string.IsNullOrEmpty(space.City))
            conditions.Add($"City = '{space.City}'");
        if (!string.IsNullOrEmpty(space.District))
            conditions.Add($"District = '{space.District}'");
        if (!string.IsNullOrEmpty(space.School))
            conditions.Add($"School = '{space.School}'");
        if (space.Grade.HasValue)
            conditions.Add($"Grade = {space.Grade}");
        if (!string.IsNullOrEmpty(space.Class))
            conditions.Add($"Class = '{space.Class}'");
        if (!string.IsNullOrEmpty(space.StudentId))
            conditions.Add($"StudentId = '{space.StudentId}'");
    }
    
    private void AddTimeConditions(List<string> conditions, TimeDimension time)
    {
        if (time.AcademicYear.HasValue)
            conditions.Add($"AcademicYear = {time.AcademicYear}");
        if (!string.IsNullOrEmpty(time.Semester))
            conditions.Add($"Semester = '{time.Semester}'");
        if (time.Month.HasValue)
            conditions.Add($"Month = {time.Month}");
        if (time.Week.HasValue)
            conditions.Add($"Week = {time.Week}");
        if (time.Date.HasValue)
            conditions.Add($"AnswerDate = '{time.Date.Value:yyyy-MM-dd}'");
        if (time.RecentDays.HasValue)
            conditions.Add($"AnswerTime >= DATEADD(day, -{time.RecentDays}, GETDATE())");
    }
    
    private void AddContentConditions(List<string> conditions, ContentDimension content)
    {
        if (!string.IsNullOrEmpty(content.Subject))
            conditions.Add($"Subject = '{content.Subject}'");
        if (!string.IsNullOrEmpty(content.Chapter))
            conditions.Add($"Chapter = '{content.Chapter}'");
        if (!string.IsNullOrEmpty(content.KnowledgePoint))
            conditions.Add($"KnowledgePoint = '{content.KnowledgePoint}'");
        if (!string.IsNullOrEmpty(content.Paper))
            conditions.Add($"Paper = '{content.Paper}'");
        if (!string.IsNullOrEmpty(content.Question))
            conditions.Add($"QuestionId = '{content.Question}'");
    }
}