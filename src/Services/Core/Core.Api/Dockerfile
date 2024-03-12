#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Services/Core/Core.Api/Core.Api.csproj", "Services/Core/Core.Api/"]
COPY ["Services/Common/Services.Common.Presentation/Services.Common.Presentation.csproj", "Services/Common/Services.Common.Presentation/"]
COPY ["Services/Core/Core.Infrastructure/Core.Infrastructure.csproj", "Services/Core/Core.Infrastructure/"]
COPY ["Services/Core/Core.Infrastructure.Persistence/Core.Infrastructure.Persistence.csproj", "Services/Core/Core.Infrastructure.Persistence/"]
COPY ["Services/Core/Core.Application/Core.Application.csproj", "Services/Core/Core.Application/"]
COPY ["Services/Core/Core.Domain/Core.Domain.csproj", "Services/Core/Core.Domain/"]
COPY ["Services/Common/Services.Common/Services.Common.csproj", "Services/Common/Services.Common/"]

RUN dotnet restore "./Services/Core/Core.Api/Core.Api.csproj"
COPY . .

WORKDIR "/src/Services/Core/Core.Api"
RUN dotnet build "./Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.Api.dll"]