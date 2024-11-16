FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8083
ENV ASPNETCORE_URLS=http://+:8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN dotnet nuget add source http://host.docker.internal:5000/v3/index.json -n baget

COPY ["src/SkillSphere.Interaction.API/SkillSphere.Interaction.API.csproj", "src/SkillSphere.Interaction.API/"]
COPY ["src/SkillSphere.Interaction.Contracts/SkillSphere.Interaction.Contracts.csproj", "src/SkillSphere.Interaction.Contracts/"]
COPY ["src/SkillSphere.Interaction.Core/SkillSphere.Interaction.Core.csproj", "src/SkillSphere.Interaction.Core/"]
COPY ["src/SkillSphere.Interaction.DataAccess/SkillSphere.Interaction.DataAccess.csproj", "src/SkillSphere.Interaction.DataAccess/"]
COPY ["src/SkillSphere.Interaction.UseCases/SkillSphere.Interaction.UseCases.csproj", "src/SkillSphere.Interaction.UseCases/"]
RUN dotnet restore "./src/SkillSphere.Interaction.API/SkillSphere.Interaction.API.csproj"

COPY . .
WORKDIR "/src/src/SkillSphere.Interaction.API"
RUN dotnet build "./SkillSphere.Interaction.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SkillSphere.Interaction.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillSphere.Interaction.API.dll"]