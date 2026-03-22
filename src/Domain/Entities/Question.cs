namespace EduAnalytics.QueryService.Domain.Entities;

public class Question
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Chapter { get; set; } = string.Empty;
    public string KnowledgePoint { get; set; } = string.Empty;
    public string Paper { get; set; } = string.Empty;
    
    // 试题属性
    public string LearningLevel { get; set; } = "理解";
    public double Difficulty { get; set; } = 0.5;
    public string ContentArea { get; set; } = "数与代数";
    public string Literacy { get; set; } = "逻辑推理";
}