FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageHosting.Storage/ImageHosting.Storage.csproj", "ImageHosting.Storage/"]
RUN dotnet restore "ImageHosting.Storage/ImageHosting.Storage.csproj"
COPY . .
WORKDIR "/src/ImageHosting.Storage"
RUN dotnet build "ImageHosting.Storage.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ImageHosting.Storage.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageHosting.Storage.dll"]
