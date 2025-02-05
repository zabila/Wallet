FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#restore
COPY [".editorconfig", "."]
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/API.Telegram/", "src/API.Telegram/"]
COPY ["src/SharedKernel/", "src/SharedKernel/"]
COPY ["src/MessageBus/", "src/MessageBus/"]
COPY ["src/Infrastructure/", "src/Infrastructure/"]
COPY ["src/Domain/", "src/Domain/"]
RUN dotnet restore "./src/API.Telegram/API.Telegram.csproj"

#build
COPY . .
WORKDIR "/src/src/API.Telegram"
RUN dotnet build "./API.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Telegram.dll"]