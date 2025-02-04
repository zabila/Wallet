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
COPY ["src/Wallet.API.Finance/", "Wallet.API.Finance/"]
COPY ["src/Wallet.SharedKernel/", "Wallet.SharedKernel/"]
COPY ["src/Wallet.Application.Finance/", "Wallet.Application.Finance/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Infrastructure.Repository/", "Wallet.Infrastructure.Repository/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]
RUN dotnet restore "Wallet.API.Finance/Wallet.API.Finance.csproj"

#build
WORKDIR /src/Wallet.API.Finance
RUN dotnet build "Wallet.API.Finance.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wallet.API.Finance.csproj" -c $BUILD_CONFIGURATION=Release -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.API.Finance.dll"]