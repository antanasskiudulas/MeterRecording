# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file
COPY ["MeterRecording.sln", "./"]

# Copy each project file
COPY ["../MeterRecording.Api/MeterRecording.Api.csproj", "MeterRecording.Api/"]
COPY ["../MeterRecording.Core/MeterRecording.Core.csproj", "MeterRecording.Core/"]
COPY ["../MeterRecording.Application/MeterRecording.Application.csproj", "MeterRecording.Application/"]
COPY ["../MeterRecording.Infrastructure/MeterRecording.Infrastructure.csproj", "MeterRecording.Infrastructure/"]

# Restore all dependencies from the solution file
RUN dotnet restore

# Copy the rest of the app source code
COPY . .

# Build the application
WORKDIR "/src/MeterRecording.Api"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ../MeterRecording.Infrastructure/Seed/Accounts/Data/Test_Accounts.csv /app/Seed/Accounts/Data/Test_Accounts.csv

ENTRYPOINT ["dotnet", "MeterRecording.Api.dll"]
