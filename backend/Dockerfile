# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy project files (alle csproj, auch Tests!)
COPY src/Sparplan.Api/Sparplan.Api.csproj src/Sparplan.Api/
COPY src/Sparplan.Domain/Sparplan.Domain.csproj src/Sparplan.Domain/
COPY src/Sparplan.Infrastructure/Sparplan.Infrastructure.csproj src/Sparplan.Infrastructure/
COPY src/Sparplan.Application/Sparplan.Application.csproj src/Sparplan.Application/
COPY tests/Sparplan.Domain.Tests/Sparplan.Domain.Tests.csproj tests/Sparplan.Domain.Tests/

# restore solution
COPY Sparplan.sln .
RUN dotnet restore Sparplan.sln

# copy rest of the source code
COPY . .

# build everything (inkl. Tests, aber ohne sie zu starten)
RUN dotnet build Sparplan.sln -c Release -o /app/build

# publish API project
RUN dotnet publish src/Sparplan.Api/Sparplan.Api.csproj -c Release -o /app/publish /p:UseAppHost=false


# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Sparplan.Api.dll"]
