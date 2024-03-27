FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wallet.Services.Telegram.csproj", "./"]
RUN dotnet restore "Wallet.Services.Telegram.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Wallet.Services.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wallet.Services.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT ["dotnet", "Wallet.Services.Telegram.dll"]
