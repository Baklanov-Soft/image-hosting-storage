# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["src/ImageHosting.Storage.WebApi/ImageHosting.Storage.WebApi.csproj", "src/ImageHosting.Storage.WebApi/"]
COPY ["src/ImageHosting.Storage.Application/ImageHosting.Storage.Application.csproj", "src/ImageHosting.Storage.Application/"]
COPY ["src/ImageHosting.Storage.Domain/ImageHosting.Storage.Domain.csproj", "src/ImageHosting.Storage.Domain/"]
COPY ["src/ImageHosting.Storage.Infrastructure/ImageHosting.Storage.Infrastructure.csproj", "src/ImageHosting.Storage.Infrastructure/"]
RUN dotnet restore "./src/ImageHosting.Storage.WebApi/ImageHosting.Storage.WebApi.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/src/ImageHosting.Storage.WebApi"
RUN dotnet build "./ImageHosting.Storage.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build -a $TARGETARCH


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ImageHosting.Storage.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:UseAppHost=false -a $TARGETARCH


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageHosting.Storage.WebApi.dll"]
