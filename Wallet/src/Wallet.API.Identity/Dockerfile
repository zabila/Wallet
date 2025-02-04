FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

#restore
COPY ["src/Wallet.API.Identity/", "Wallet.API.Identity/"]
COPY ["src/Wallet.Shared/", "Wallet.Shared/"]
COPY ["src/Wallet.Application.Identity/", "Wallet.Application.Identity/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Infrastructure.Repository/", "Wallet.Infrastructure.Repository/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]

RUN dotnet restore "Wallet.API.Identity/Wallet.API.Identity.csproj"

#build
WORKDIR /src/Wallet.API.Identity
RUN dotnet build "Wallet.API.Identity.csproj" -c Release -o /app/build

#Stage: Publish
FROM build AS publish
RUN dotnet publish "Wallet.API.Identity.csproj" -c Release -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Create a non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
# Switch to non-root user
USER appuser

ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.API.Identity.dll"]