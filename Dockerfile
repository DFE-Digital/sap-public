# =====================================================
# Stage 0: Set version arguments
# =====================================================
ARG DOTNET_VERSION=8.0
ARG NODEJS_VERSION_MAJOR=22


# =====================================================
# Stage 1: Build frontend assets (DfE / GOV.UK)
# =====================================================
FROM node:${NODEJS_VERSION_MAJOR}-bullseye-slim AS assets
WORKDIR /app

# Copy package files for dependency installation
COPY ./SAPPub.Web/package*.json /app/

# Install dependencies - this will trigger postinstall which runs copy-assets
# The postinstall script copies dfe-frontend and govuk-frontend from node_modules to wwwroot/lib
RUN npm ci

# Copy all wwwroot contents (custom assets, images, CSS, etc.)
COPY ./SAPPub.Web/wwwroot/ /app/wwwroot/

# Debug: Show what was built and where
RUN echo "=== Assets build output ===" && \
    echo "Checking wwwroot structure:" && \
    find /app/wwwroot -type d | head -20 && \
    echo "=== Checking for frontend libraries ===" && \
    ls -la /app/wwwroot/lib/ 2>/dev/null || echo "No lib directory yet" && \
    ls -la /app/wwwroot/lib/dfe-frontend/ 2>/dev/null || echo "DfE frontend not in wwwroot/lib" && \
    ls -la /app/wwwroot/lib/govuk-frontend/ 2>/dev/null || echo "GOV.UK frontend not in wwwroot/lib"


# =====================================================
# Stage 2: Build .NET project
# =====================================================
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore packages
COPY ["SAPPub.Web/SAPPub.Web.csproj", "SAPPub.Web/"]
COPY ["SAPPub.Core/SAPPub.Core.csproj", "SAPPub.Core/"]
COPY ["SAPPub.Infrastructure/SAPPub.Infrastructure.csproj", "SAPPub.Infrastructure/"]
RUN dotnet restore "./SAPPub.Web/SAPPub.Web.csproj"

# Copy all code
COPY . .

# Build the project
WORKDIR "/src/SAPPub.Web"
RUN dotnet build "./SAPPub.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SAPPub.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# =====================================================
# Stage 3: Runtime image
# =====================================================
#FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled AS final

WORKDIR /app

# Create a writable folder for Data Protection keys
#RUN mkdir -p /keys && chmod -R 777 /keys
RUN mkdir -p /keys && chown -R app:app /keys

# Environment configuration
ENV ASPNETCORE_URLS=http://+:3000

# Copy published .NET app
COPY --from=publish /app/publish .

# Copy frontend assets from the assets stage (will merge with published wwwroot)
COPY --from=assets /app/wwwroot ./wwwroot

# Debug: Verify files are present
RUN echo "=== Final wwwroot contents ===" && \
    ls -la /app/wwwroot/ && \
    echo "=== Checking for frontend libraries ===" && \
    ls -la /app/wwwroot/lib/ 2>/dev/null || echo "No lib directory" && \
    echo "=== Checking for DfE frontend ===" && \
    ls -la /app/wwwroot/lib/dfe-frontend/ 2>/dev/null || echo "DfE not found"

# Expose port
EXPOSE 3000

# Important: switch to non-root user *after* permissions are set
#USER $APP_UID

# Entry point
ENTRYPOINT ["dotnet", "SAPPub.Web.dll"]