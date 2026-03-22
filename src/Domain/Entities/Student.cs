namespace EduAnalytics.QueryService.Domain.Entities;

public class Student
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public int Grade { get; set; }
    public string Class { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}