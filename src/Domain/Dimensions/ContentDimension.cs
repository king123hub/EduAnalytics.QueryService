namespace EduAnalytics.QueryService.Domain.Dimensions;

/// <summary>
/// 内容维度：学科/章节单元/知识点/试卷/试题
/// </summary>
public class ContentDimension
{
    public string? Subject { get; set; }
    public string? Chapter { get; set; }
    public string? KnowledgePoint { get; set; }
    public string? Paper { get; set; }
    public string? Question { get; set; }
    public QuestionAttribute? QuestionAttr { get; set; }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(Subject) && 
               string.IsNullOrEmpty(Chapter) && 
               string.IsNullOrEmpty(KnowledgePoint) && 
               string.IsNullOrEmpty(Paper) && 
               string.IsNullOrEmpty(Question) && 
               QuestionAttr == null;
    }
}

/// <summary>
/// 试题属性
/// </summary>
public class QuestionAttribute
{
    public string? LearningLevel { get; set; }  // 学习水平
    public double? Difficulty { get; set; }     // 难度系数
    public string? ContentArea { get; set; }    // 内容领域
    public string? Literacy { get; set; }       // 素养表现
}