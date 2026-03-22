using EduAnalytics.QueryService.Domain.Entities;
using EduAnalytics.QueryService.Infrastructure.Data;

namespace EduAnalytics.QueryService.Infrastructure.MockData;

public static class MockDataSeeder
{
    public static async Task SeedAsync(EduDbContext context)
    {
        if (context.Students.Any() || context.Questions.Any() || context.StudentAnswers.Any())
            return;

        // 1. 创建学生数据（50个）
        var students = GenerateStudents(50);
        await context.Students.AddRangeAsync(students);

        // 2. 创建试题数据（20道）
        var questions = GenerateQuestions(20);
        await context.Questions.AddRangeAsync(questions);

        // 3. 创建作答记录（500条）
        var answers = GenerateAnswers(students, questions, 500);
        await context.StudentAnswers.AddRangeAsync(answers);

        await context.SaveChangesAsync();
    }

    private static List<Student> GenerateStudents(int count)
    {
        var students = new List<Student>();
        var schools = new[] { "朝阳一小", "朝阳二小", "朝阳实验中学" };
        var districts = new[] { "朝阳区", "海淀区", "东城区" };
        var random = new Random(42);

        for (int i = 1; i <= count; i++)
        {
            var schoolIndex = random.Next(schools.Length);
            students.Add(new Student
            {
                Id = $"S-{i:D4}",
                Name = $"学生{i}",
                School = schools[schoolIndex],
                Grade = random.Next(1, 9),
                Class = $"{random.Next(1, 6)}班",
                District = districts[random.Next(districts.Length)],
                City = "北京市"
            });
        }

        return students;
    }

    private static List<Question> GenerateQuestions(int count)
    {
        var questions = new List<Question>();
        var subjects = new[] { "数学", "语文", "英语" };
        var chapters = new[] { "二次函数", "一元二次方程", "几何证明", "文言文", "阅读理解", "时态语态" };
        var papers = new[] { "期中考试", "期末考试", "单元测试" };
        var random = new Random(42);

        for (int i = 1; i <= count; i++)
        {
            var subjectIndex = random.Next(subjects.Length);
            questions.Add(new Question
            {
                Id = $"Q-{i:D4}",
                Content = $"试题内容{i}",
                Subject = subjects[subjectIndex],
                Chapter = chapters[random.Next(chapters.Length)],
                KnowledgePoint = $"知识点{i}",
                Paper = $"{papers[random.Next(papers.Length)]}卷{i % 3 + 1}",
                LearningLevel = random.Next(3) switch
                {
                    0 => "理解",
                    1 => "应用",
                    _ => "分析"
                },
                Difficulty = Math.Round(random.NextDouble() * 0.5 + 0.3, 2),
                ContentArea = random.Next(3) switch
                {
                    0 => "数与代数",
                    1 => "图形与几何",
                    _ => "统计与概率"
                },
                Literacy = random.Next(3) switch
                {
                    0 => "逻辑推理",
                    1 => "数学建模",
                    _ => "数据分析"
                }
            });
        }

        return questions;
    }

    private static List<StudentAnswer> GenerateAnswers(List<Student> students, List<Question> questions, int count)
    {
        var answers = new List<StudentAnswer>();
        var random = new Random(42);
        var startDate = new DateTime(2024, 9, 1);

        for (int i = 1; i <= count; i++)
        {
            var student = students[random.Next(students.Count)];
            var question = questions[random.Next(questions.Count)];
            var answerDate = startDate.AddDays(random.Next(180));
            var isCorrect = random.NextDouble() > 0.3; // 70%正确率
            var score = isCorrect ? 100 : random.Next(30, 60);

            answers.Add(new StudentAnswer
            {
                Id = $"A-{i:D4}",
                StudentId = student.Id,
                QuestionId = question.Id,
                Score = score,
                IsCorrect = isCorrect,
                AnswerTime = answerDate,
                
                // 冗余学生信息
                StudentName = student.Name,
                School = student.School,
                Grade = student.Grade,
                Class = student.Class,
                District = student.District,
                
                // 冗余试题信息
                Subject = question.Subject,
                Chapter = question.Chapter,
                KnowledgePoint = question.KnowledgePoint,
                Paper = question.Paper,
                
                // 时间维度
                AcademicYear = answerDate.Year >= 9 ? answerDate.Year : answerDate.Year - 1,
                Semester = answerDate.Month >= 9 ? "上学期" : "下学期"
            });
        }

        return answers;
    }
}