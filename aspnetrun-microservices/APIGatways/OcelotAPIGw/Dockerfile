#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["APIGatways/OcelotAPIGw/OcelotAPIGw.csproj", "APIGatways/OcelotAPIGw/"]
RUN dotnet restore "APIGatways/OcelotAPIGw/OcelotAPIGw.csproj"
COPY . .
WORKDIR "/src/APIGatways/OcelotAPIGw"
RUN dotnet build "OcelotAPIGw.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotAPIGw.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotAPIGw.dll"]