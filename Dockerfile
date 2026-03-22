# 使用.NET 8 SDK作为构建镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制项目文件并还原依赖
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Api/Api.csproj src/Api/

RUN dotnet restore src/Api/Api.csproj

# 复制所有源代码并构建
COPY . .
RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish

# 构建运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 从构建阶段复制发布文件
COPY --from=build /app/publish .

# 设置环境变量
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080

# 创建非root用户运行
USER $APP_UID

ENTRYPOINT ["dotnet", "Api.dll"]