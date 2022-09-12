#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Raven.Projects.API/Raven.Projects.API.csproj", "Raven.Projects.API/"]
RUN dotnet restore "Raven.Projects.API/Raven.Projects.API.csproj"
COPY . .
WORKDIR "/src/Raven.Projects.API"
RUN dotnet build "Raven.Projects.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raven.Projects.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raven.Projects.API.dll"]