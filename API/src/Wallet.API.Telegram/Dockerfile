#Stage: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#restore
COPY ["src/Wallet.API.Telegram/", "Wallet.API.Telegram/"]
COPY ["src/Wallet.Shared/", "Wallet.Shared/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Infrastructure.Repository/", "Wallet.Infrastructure.Repository/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]

RUN dotnet restore "Wallet.API.Telegram/Wallet.API.Telegram.csproj"

#build
WORKDIR /src/Wallet.API.Telegram
RUN dotnet build "Wallet.API.Telegram.csproj" -c Release -o /app/build

#Stage: Publish
FROM build AS publish
RUN dotnet publish "Wallet.API.Telegram.csproj" -c Release -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.API.Telegram.dll"]