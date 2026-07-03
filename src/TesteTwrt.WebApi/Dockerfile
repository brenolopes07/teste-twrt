FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/TesteTwrt.WebApi/TesteTwrt.WebApi.csproj", "src/TesteTwrt.WebApi/"]
COPY ["src/TesteTwrt.Application/TesteTwrt.Application.csproj", "src/TesteTwrt.Application/"]
COPY ["src/TesteTwrt.Infrastructure/TesteTwrt.Infrastructure.csproj", "src/TesteTwrt.Infrastructure/"]
COPY ["src/TesteTwrt.Domain/TesteTwrt.Domain.csproj", "src/TesteTwrt.Domain/"]

RUN dotnet restore "src/TesteTwrt.WebApi/TesteTwrt.WebApi.csproj"

COPY . .
WORKDIR "/src/src/TesteTwrt.WebApi"
RUN dotnet build "TesteTwrt.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TesteTwrt.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TesteTwrt.WebApi.dll"]
