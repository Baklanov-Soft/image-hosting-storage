# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["src/ImageHosting.Storage.Tagger/ImageHosting.Storage.Tagger.csproj", "src/ImageHosting.Storage.Tagger/"]
COPY ["src/ImageHosting.Storage.Infrastructure/ImageHosting.Storage.Infrastructure.csproj", "src/ImageHosting.Storage.Infrastructure/"]
COPY ["src/ImageHosting.Storage.Application/ImageHosting.Storage.Application.csproj", "src/ImageHosting.Storage.Application/"]
COPY ["src/ImageHosting.Storage.Domain/ImageHosting.Storage.Domain.csproj", "src/ImageHosting.Storage.Domain/"]
RUN dotnet restore "./src/ImageHosting.Storage.Tagger/ImageHosting.Storage.Tagger.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/src/ImageHosting.Storage.Tagger"
RUN dotnet build "./ImageHosting.Storage.Tagger.csproj" -c $BUILD_CONFIGURATION -o /app/build -a $TARGETARCH


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ImageHosting.Storage.Tagger.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:UseAppHost=false -a $TARGETARCH


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageHosting.Storage.Tagger.dll"]
