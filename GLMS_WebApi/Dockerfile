# ──────────────────────────────────────────────────────────────────────
# Stage 1: Restore & Build
# ──────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for layer caching
COPY ["GLMS_WebApi/GLMS_WebApi.csproj", "GLMS_WebApi/"]
COPY ["GLMS.Core/GLMS.Core.csproj", "GLMS.Core/"]   
RUN dotnet restore "GLMS_WebApi/GLMS_WebApi.csproj"

# Copy full source
COPY . .

# Build & publish (Release, no restore needed)
WORKDIR /src/GLMS_WebApi
RUN dotnet publish "GLMS_WebApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ──────────────────────────────────────────────────────────────────────
# Stage 2: Runtime (minimal image)
# ──────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "GLMS_WebApi.dll"]