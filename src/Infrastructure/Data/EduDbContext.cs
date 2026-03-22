using Microsoft.EntityFrameworkCore;
using EduAnalytics.QueryService.Domain.Entities;

namespace EduAnalytics.QueryService.Infrastructure.Data;

public class EduDbContext : DbContext
{
    public EduDbContext(DbContextOptions<EduDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置学生实体
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(50);
            entity.HasIndex(e => e.School);
            entity.HasIndex(e => new { e.District, e.School });
        });

        // 配置试题实体
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(50);
            entity.HasIndex(e => e.Subject);
            entity.HasIndex(e => e.Chapter);
            entity.HasIndex(e => e.Paper);
        });

        // 配置学生作答实体（宽表模型）
        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(50);
            
            // 空间维度索引
            entity.HasIndex(e => new { e.District, e.School, e.Grade, e.Class });
            
            // 时间维度索引
            entity.HasIndex(e => e.AnswerTime);
            entity.HasIndex(e => new { e.AcademicYear, e.Semester });
            
            // 内容维度索引
            entity.HasIndex(e => new { e.Subject, e.Chapter, e.KnowledgePoint });
            entity.HasIndex(e => e.Paper);
            entity.HasIndex(e => e.QuestionId);
            
            // 复合索引（常用查询组合）
            entity.HasIndex(e => new { 
                e.District, 
                e.Grade, 
                e.Subject, 
                e.AcademicYear, 
                e.Semester 
            });
        });
    }
}