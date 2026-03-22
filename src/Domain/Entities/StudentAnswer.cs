namespace EduAnalytics.QueryService.Domain.Entities;

public class StudentAnswer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string StudentId { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public double Score { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime AnswerTime { get; set; }
    
    // 宽表冗余字段（避免JOIN）
    public string StudentName { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public int Grade { get; set; }
    public string Class { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    
    public string Subject { get; set; } = string.Empty;
    public string Chapter { get; set; } = string.Empty;
    public string KnowledgePoint { get; set; } = string.Empty;
    public string Paper { get; set; } = string.Empty;
    
    public int AcademicYear { get; set; }
    public string Semester { get; set; } = string.Empty;
}