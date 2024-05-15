#Stage: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

#restore
COPY ["src/Wallet.API.Finance/", "Wallet.API.Finance/"]
COPY ["src/Wallet.Shared/", "Wallet.Shared/"]
COPY ["src/Wallet.Application.Finance/", "Wallet.Application.Finance/"]
COPY ["src/Wallet.Infrastructure.LoggerService/", "Wallet.Infrastructure.LoggerService/"]
COPY ["src/Wallet.Infrastructure.Repository/", "Wallet.Infrastructure.Repository/"]
COPY ["src/Wallet.Integration.MessageBus/", "Wallet.Integration.MessageBus/"]
COPY ["src/Wallet.Domain.Contracts/", "Wallet.Domain.Contracts/"]
COPY ["src/Wallet.Domain.Entities/", "Wallet.Domain.Entities/"]

RUN dotnet restore "Wallet.API.Finance/Wallet.API.Finance.csproj"

#build
WORKDIR /src/Wallet.API.Finance
RUN dotnet build "Wallet.API.Finance.csproj" -c Release -o /app/build

#Stage: Publish
FROM build AS publish
RUN dotnet publish "Wallet.API.Finance.csproj" -c Release -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Create a non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
# Switch to non-root user
USER appuser

ENV ASPNERCORE_HTTPS_PORT=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.API.Finance.dll"]