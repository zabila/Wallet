FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

#restore
COPY ["src/Wallet.Services.Telegram/", "Wallet.Services.Telegram/"]
COPY ["src/Wallet.Shared/", "Wallet.Shared/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]

RUN dotnet restore "Wallet.Services.Telegram/Wallet.Services.Telegram.csproj"

#build
WORKDIR /src/Wallet.Services.Telegram
RUN dotnet build "Wallet.Services.Telegram.csproj" -c Release -o /app/build

#Stage: Publish
FROM build AS publish
RUN dotnet publish "Wallet.Services.Telegram.csproj" -c Release -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Create a non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
# Switch to non-root user
USER appuser

ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.Services.Telegram.dll"]