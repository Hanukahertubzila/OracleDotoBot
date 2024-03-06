#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OracleDotoBot/OracleDotoBot.csproj", "OracleDotoBot/"]
COPY ["Domain/OracleDotoBot.Domain.csproj", "Domain/"]
COPY ["OracleDotoBot.SteamApi/OracleDotoBot.SteamApi.csproj", "OracleDotoBot.SteamApi/"]
COPY ["OracleDotoBot.StratzApiParser/OracleDotoBot.StratzApi.csproj", "OracleDotoBot.StratzApiParser/"]
RUN dotnet restore "OracleDotoBot/OracleDotoBot.csproj"
COPY . .
WORKDIR "/src/OracleDotoBot"
RUN dotnet build "OracleDotoBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OracleDotoBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OracleDotoBot.dll"]