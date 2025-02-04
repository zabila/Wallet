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
COPY ["src/Wallet.API.Telegram/", "Wallet.API.Telegram/"]
COPY ["src/Wallet.SharedKernel/", "Wallet.SharedKernel/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Infrastructure.Repository/", "Wallet.Infrastructure.Repository/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]
RUN dotnet restore "Wallet.API.Telegram/Wallet.API.Telegram.csproj"

#build
WORKDIR /src/Wallet.API.Telegram
RUN dotnet build "Wallet.API.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wallet.API.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.API.Telegram.dll"]