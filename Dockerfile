# Build and test using the official .NET 8 SDK alpine image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy solution files
COPY . .

# Restore dependencies for both main and test projects
RUN dotnet restore ./src/notification.engine.csproj && \
    dotnet restore ./test/notification.engine.test/notification.engine.test.csproj

# Build both projects
RUN dotnet build ./src/notification.engine.csproj -c Release --no-restore && \
    dotnet build ./test/notification.engine.test/notification.engine.test.csproj -c Release --no-restore

# Run tests (fail build if tests fail)
RUN dotnet test ./test/notification.engine.test/notification.engine.test.csproj -c Release --no-build --logger:trx

# Publish the main app
RUN dotnet publish ./src/notification.engine.csproj -c Release -o /app/publish --no-restore

# .NET 8 ASP.NET alpine image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Create a non-root user and group for least privilege
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

WORKDIR /app
COPY --from=build /app/publish .

# Set permissions for the app directory
RUN chown -R appuser:appgroup /app

# Expose the default port
EXPOSE 8080

# Set environment variables for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Drop privileges to non-root user
USER appuser

ENTRYPOINT ["dotnet", "notification.engine.dll"]