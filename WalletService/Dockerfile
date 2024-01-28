FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /source
EXPOSE 80

COPY *.sln .
COPY ["Application/Application.csproj", "Application/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["WalletService/WalletService.csproj", "WalletService/"]
COPY ["LoggerService/LoggerService.csproj", "LoggerService/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["WalletService.Presentation/WalletService.Presentation.csproj", "WalletService.Presentation/"]
RUN dotnet restore

COPY . ./
WORKDIR /source/WalletService
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app ./
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT [ "dotnet", "WalletService.dll" ]


