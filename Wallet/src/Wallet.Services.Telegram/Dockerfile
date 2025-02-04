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
COPY ["src/Wallet.Services.Telegram/", "Wallet.Services.Telegram/"]
COPY ["src/Wallet.SharedKernel/", "Wallet.SharedKernel/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]
RUN dotnet restore "Wallet.Services.Telegram/Wallet.Services.Telegram.csproj"

#build
WORKDIR /src/Wallet.Services.Telegram
RUN dotnet build "Wallet.Services.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wallet.Services.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.Services.Telegram.dll"]