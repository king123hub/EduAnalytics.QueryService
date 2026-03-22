namespace EduAnalytics.QueryService.Domain.Dimensions;

/// <summary>
/// 空间维度：市/区/学校/年级/班级/学生
/// </summary>
public class SpaceDimension
{
    public string? City { get; set; }
    public string? District { get; set; }
    public string? School { get; set; }
    public int? Grade { get; set; }
    public string? Class { get; set; }
    public string? StudentId { get; set; }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(City) && 
               string.IsNullOrEmpty(District) && 
               string.IsNullOrEmpty(School) && 
               !Grade.HasValue && 
               string.IsNullOrEmpty(Class) && 
               string.IsNullOrEmpty(StudentId);
    }

    public override string ToString()
    {
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(City)) parts.Add($"City={City}");
        if (!string.IsNullOrEmpty(District)) parts.Add($"District={District}");
        if (!string.IsNullOrEmpty(School)) parts.Add($"School={School}");
        if (Grade.HasValue) parts.Add($"Grade={Grade}");
        if (!string.IsNullOrEmpty(Class)) parts.Add($"Class={Class}");
        if (!string.IsNullOrEmpty(StudentId)) parts.Add($"StudentId={StudentId}");
        return string.Join(", ", parts);
    }
}