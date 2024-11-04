# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SkillSphere.Interaction.API/SkillSphere.Interaction.API.csproj", "src/SkillSphere.Interaction.API/"]
COPY ["src/SkillSphere.Interaction.Contracts/SkillSphere.Interaction.Contracts.csproj", "src/SkillSphere.Interaction.Contracts/"]
COPY ["src/SkillSphere.Interaction.Core/SkillSphere.Interaction.Core.csproj", "src/SkillSphere.Interaction.Core/"]
COPY ["src/SkillSphere.Interaction.DataAccess/SkillSphere.Interaction.DataAccess.csproj", "src/SkillSphere.Interaction.DataAccess/"]
COPY ["src/SkillSphere.Interaction.UseCases/SkillSphere.Interaction.UseCases.csproj", "src/SkillSphere.Interaction.UseCases/"]
RUN dotnet restore "./src/SkillSphere.Interaction.API/SkillSphere.Interaction.API.csproj"
COPY . .
WORKDIR "/src/src/SkillSphere.Interaction.API"
RUN dotnet build "./SkillSphere.Interaction.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SkillSphere.Interaction.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillSphere.Interaction.API.dll"]