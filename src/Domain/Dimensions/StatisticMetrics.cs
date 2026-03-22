namespace EduAnalytics.QueryService.Domain.Dimensions;

/// <summary>
/// 统计指标：平均分/及格率/正确率/排名等
/// </summary>
public class StatisticMetrics
{
    public double? AvgScore { get; set; }
    public double? PassRate { get; set; }
    public double? ExcellentRate { get; set; }
    public double? CorrectRate { get; set; }
    public double? ErrorRate { get; set; }
    public int? AnswerCount { get; set; }
    public double? Trend { get; set; }
    public int? Rank { get; set; }
    public int? TotalQuestions { get; set; }
    public int? TotalPapers { get; set; }

    public static StatisticMetrics FromDictionary(Dictionary<string, object> data)
    {
        var metrics = new StatisticMetrics();
        if (data.ContainsKey("AvgScore")) metrics.AvgScore = Convert.ToDouble(data["AvgScore"]);
        if (data.ContainsKey("PassRate")) metrics.PassRate = Convert.ToDouble(data["PassRate"]);
        if (data.ContainsKey("CorrectRate")) metrics.CorrectRate = Convert.ToDouble(data["CorrectRate"]);
        return metrics;
    }
}