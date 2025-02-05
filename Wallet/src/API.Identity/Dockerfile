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
COPY ["src/API.Identity/", "src/API.Identity/"]
COPY ["src/SharedKernel/", "src/SharedKernel/"]
COPY ["src/Application/", "src/Application/"]
COPY ["src/Domain/", "src/Domain/"]
COPY ["src/Infrastructure/", "src/Infrastructure/"]
RUN dotnet restore "./src/API.Identity/API.Identity.csproj"

#build
COPY . .
WORKDIR "/src/src/API.Identity"
RUN dotnet build "./API.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#Stage: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNERCORE_HTTPS_PORT=5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Identity.dll"]