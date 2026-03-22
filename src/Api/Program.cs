using EduAnalytics.QueryService.Application.IntentParser;
using EduAnalytics.QueryService.Application.SqlGenerator;
using EduAnalytics.QueryService.Application.QueryExecutor;
using EduAnalytics.QueryService.Infrastructure.Data;
using EduAnalytics.QueryService.Infrastructure.MockData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EduAnalytics.QueryService API",
        Version = "v1",
        Description = "教育数据智能分析查询服务 - 多维度NL2SQL查询POC"
    });
});

// 添加DbContext（使用内存数据库，便于演示）
builder.Services.AddDbContext<EduDbContext>(options =>
    options.UseInMemoryDatabase("EduAnalyticsDB"));

// 注册应用服务
builder.Services.AddScoped<IIntentParser, MockLLMParser>();
builder.Services.AddScoped<ISqlGenerator, EduSqlGenerator>();
builder.Services.AddScoped<IQueryExecutor, InMemoryQueryExecutor>();

// 添加日志
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// 初始化数据库并填充Mock数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EduDbContext>();
    await MockDataSeeder.SeedAsync(context);
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Mock数据初始化完成");
}

// 配置中间件
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();