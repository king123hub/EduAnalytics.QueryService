namespace EduAnalytics.QueryService.Domain.Dimensions;

/// <summary>
/// 时间维度：学年/学期/月/周/日/最近N天
/// </summary>
public class TimeDimension
{
    public int? AcademicYear { get; set; }
    public string? Semester { get; set; }  // 上学期/下学期
    public int? Month { get; set; }
    public int? Week { get; set; }
    public DateTime? Date { get; set; }
    public int? RecentDays { get; set; }

    public bool IsEmpty()
    {
        return !AcademicYear.HasValue && 
               string.IsNullOrEmpty(Semester) && 
               !Month.HasValue && 
               !Week.HasValue && 
               !Date.HasValue && 
               !RecentDays.HasValue;
    }
}