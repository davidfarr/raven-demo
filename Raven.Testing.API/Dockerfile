#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Raven.Testing.API/Raven.Testing.API.csproj", "Raven.Testing.API/"]
RUN dotnet restore "Raven.Testing.API/Raven.Testing.API.csproj"
COPY . .
WORKDIR "/src/Raven.Testing.API"
RUN dotnet build "Raven.Testing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raven.Testing.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raven.Testing.API.dll"]