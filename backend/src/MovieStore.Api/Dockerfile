FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MovieStore.Api/MovieStore.Api.csproj", "src/MovieStore.Api/"]
COPY ["src/MovieStore.Domain/MovieStore.Domain.csproj", "src/MovieStore.Domain/"]
COPY ["src/MovieStore.Application/MovieStore.Application.csproj", "src/MovieStore.Application/"]
COPY ["src/MovieStore.Infrastructure/MovieStore.Infrastructure.csproj", "src/MovieStore.Infrastructure/"]
RUN dotnet restore "src/MovieStore.Api/MovieStore.Api.csproj"
COPY . .
WORKDIR "/src/src/MovieStore.Api"
RUN dotnet build "./MovieStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieStore.Api.dll"]
