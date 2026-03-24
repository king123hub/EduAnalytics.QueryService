
### 核心流程
1. **意图解析**：模拟大模型将自然语言解析为结构化的查询意图
2. **SQL生成**：根据意图生成优化的查询SQL
3. **查询执行**：在宽表上执行查询并返回统计指标
4. **智能下钻**：识别"为什么"类问题，自动下钻定位根因

## 核心功能

| 功能 | 说明 | 示例 |
|------|------|------|
| 多维度建模 | 空间、时间、内容、统计四大维度 | 市/区/学校/年级/班级、学年/学期/月/周/日 |
| NL2SQL解析 | 自然语言转结构化意图 | "朝阳区八年级数学平均分" → 解析为Score分析类型 |
| SQL生成器 | 根据意图生成优化SQL | 自动添加WHERE、GROUP BY、ORDER BY |
| 智能下钻 | 追问"为什么"时自动下钻 | 学科→章节→试题，定位根因 |
| 统计指标 | 支持多种聚合指标 | 平均分、及格率、正确率、排名、趋势 |

## 维度设计

### 空间维度
- 市 / 区 / 学校 / 年级 / 班级 / 学生

### 时间维度
- 学年 / 学期 / 月 / 周 / 日 / 最近N天

### 内容维度
- 学科 / 章节单元 / 知识点 / 试卷 / 试题
- 试题属性：学习水平、难度系数、内容领域、素养表现

### 统计指标
- 平均分、及格率、优秀率、正确率、错误率
- 答题数、趋势、排名
- 内容维度总数（试题总数、试卷总数）

## 架构决策与权衡

| 决策点 | 选择方案 | 权衡说明 |
|--------|----------|----------|
| 查询引擎 | 宽表 + NL2SQL | **vs 向量数据库**：牺牲语义搜索灵活性，换取精确聚合确定性。教育分析需要精确数值（平均分、及格率），向量适合语义相似度，不适合精确聚合。 |
| NL2SQL方式 | 大模型解析为JSON | **vs 直接生成SQL**：增加中间层开销，但获得安全性和可控性。可校验维度/指标合法性，防止SQL注入。 |
| 数据模型 | 宽表 | **vs 星型模型**：牺牲维度扩展性，换取查询性能。将复杂的星型模型提前物化，查询时只需简单SELECT，保证亿级数据秒级响应。 |
| 下钻实现 | 预定义下钻路径 | **vs 动态解析**：牺牲路径灵活性，保证分析逻辑的可解释性。下钻路径与业务语义强绑定，用户更容易理解。 |

## AI协作说明

本项目使用 **Cursor** 辅助生成了部分基础设施代码，包括：
- Entity Framework Core的DbContext配置
- Controller基础骨架
- 部分DTO类定义

**人工主导的关键调整**：
1. **维度映射配置化**：将硬编码的字段映射改为可扩展的配置结构，支持新增维度无需修改核心代码
2. **SQL生成器安全加固**：从字符串拼接改为参数化查询，防止SQL注入风险
3. **意图解析器优化**：重新设计JSON Schema结构，确保覆盖所有分析类型（成绩/掌握/错题/趋势/对比）
4. **下钻路径算法**：完全手动实现下钻逻辑，确保与业务语义（学科→章节→试题）一致

## 项目结构

```
EduAnalytics.QueryService/
├── src/
│   ├── Api/                          # Web API层
│   │   ├── Controllers/
│   │   │   └── AnalyticsController.cs    # 查询接口（自然语言查询、智能下钻）
│   │   ├── Program.cs                    # 应用启动入口、依赖注入配置
│   │   └── appsettings.json              # 配置文件
│   │
│   ├── Application/                  # 应用服务层
│   │   ├── IntentParser/             # 意图解析模块
│   │   │   └── Models/
│   │   │       ├── IIntentParser.cs      # 意图解析器接口
│   │   │       ├── MockLLMParser.cs      # 模拟大模型解析器
│   │   │       ├── ParsedIntent.cs       # 结构化意图模型
│   │   │       └── AnalysisType.cs       # 分析类型枚举
│   │   │
│   │   ├── SqlGenerator/             # SQL生成模块
│   │   │   ├── ISqlGenerator.cs          # SQL生成器接口
│   │   │   ├── EduSqlGenerator.cs        # 教育领域SQL生成器
│   │   │   └── DimensionMapper.cs        # 维度字段映射配置
│   │   │
│   │   └── QueryExecutor/            # 查询执行模块
│   │       └── IQueryExecutor.cs         # 查询执行器接口
│   │
│   ├── Domain/                       # 领域模型层
│   │   ├── Entities/                 # 实体定义
│   │   │   ├── Student.cs                # 学生实体
│   │   │   ├── Question.cs               # 试题实体
│   │   │   └── StudentAnswer.cs          # 学生作答实体（宽表）
│   │   │
│   │   └── Dimensions/               # 维度模型
│   │       ├── SpaceDimension.cs         # 空间维度（市/区/学校/年级/班级）
│   │       ├── TimeDimension.cs          # 时间维度（学年/学期/月/周/日）
│   │       ├── ContentDimension.cs       # 内容维度（学科/章节/知识点/试题）
│   │       └── StatisticMetrics.cs       # 统计指标（平均分/及格率/正确率）
│   │
│   └── Infrastructure/               # 基础设施层
│       ├── Data/
│       │   ├── EduDbContext.cs           # EF Core数据库上下文
│       │   └── InMemoryQueryExecutor.cs  # 内存数据库查询执行器
│       │
│       └── MockData/
│           └── MockDataSeeder.cs         # Mock数据填充器
│
├── docker-compose.yml                # Docker编排配置
├── Dockerfile                        # Docker镜像构建文件
└── README.md                         # 项目说明文档
```

### 分层说明

| 层级 | 职责 | 依赖关系 |
|------|------|----------|
| **Api** | 接收HTTP请求，返回JSON响应 | → Application |
| **Application** | 业务逻辑编排（意图解析→SQL生成→查询执行） | → Domain, Infrastructure |
| **Domain** | 领域模型和业务规则（实体、维度定义） | 无依赖（纯领域层） |
| **Infrastructure** | 数据访问、外部服务集成 | → Domain |

### 核心模块

1. **IntentParser（意图解析器）**
   - 将自然语言问题解析为结构化的 `ParsedIntent` 对象
   - 当前使用 `MockLLMParser` 模拟大模型解析
   - 生产环境可替换为真实的LLM调用（OpenAI/Claude/本地模型）

2. **SqlGenerator（SQL生成器）**
   - 根据 `ParsedIntent` 生成优化的SQL查询语句
   - 通过 `DimensionMapper` 配置化管理维度字段映射
   - 自动添加 WHERE、GROUP BY、ORDER BY 子句

3. **QueryExecutor（查询执行器）**
   - 执行SQL并返回结果集
   - 当前使用内存数据库（EF Core InMemory）
   - 生产环境可切换为SQL Server/PostgreSQL

4. **宽表模型（StudentAnswer）**
   - 将星型模型提前物化为宽表，包含所有维度字段
   - 空间维度：City, District, School, Grade, Class, StudentId
   - 时间维度：AcademicYear, Semester, Month, Week, AnswerTime
   - 内容维度：Subject, Chapter, KnowledgePoint, Paper, QuestionId
   - 统计字段：Score, IsCorrect, FullScore
