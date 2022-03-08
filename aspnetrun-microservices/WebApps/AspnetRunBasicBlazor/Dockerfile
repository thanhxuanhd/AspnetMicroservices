#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApps/AspnetRunBasicBlazor/AspnetRunBasicBlazor.csproj", "WebApps/AspnetRunBasicBlazor/"]
RUN dotnet restore "WebApps/AspnetRunBasicBlazor/AspnetRunBasicBlazor.csproj"
COPY . .
WORKDIR "/src/WebApps/AspnetRunBasicBlazor"
RUN dotnet build "AspnetRunBasicBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspnetRunBasicBlazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspnetRunBasicBlazor.dll"]
