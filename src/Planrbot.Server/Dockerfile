#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Planrbot.Server/Planrbot.Server.csproj", "src/Planrbot.Server/"]
COPY ["src/Planrbot.Models/Planrbot.Models.csproj", "src/Planrbot.Models/"]
RUN dotnet restore "src/Planrbot.Server/Planrbot.Server.csproj"
COPY . .
WORKDIR "/src/src/Planrbot.Server"
RUN dotnet build "Planrbot.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planrbot.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planrbot.Server.dll"]