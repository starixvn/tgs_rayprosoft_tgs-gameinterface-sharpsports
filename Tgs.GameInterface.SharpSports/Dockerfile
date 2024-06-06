#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tgs.GameInterface.SharpSports/Tgs.GameInterface.SharpSports.csproj", "Tgs.GameInterface.SharpSports/"]
COPY ["Tgs.GameInterface.SharpSports.Core/Tgs.GameInterface.SharpSports.Core.csproj", "Tgs.GameInterface.SharpSports.Core/"]
COPY ["Tgs.GameInterface.SharpSports.Protos/Tgs.GameInterface.SharpSports.Protos.csproj", "Tgs.GameInterface.SharpSports.Protos/"]
RUN dotnet restore "./Tgs.GameInterface.SharpSports/Tgs.GameInterface.SharpSports.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.SharpSports"
RUN dotnet build "./Tgs.GameInterface.SharpSports.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.SharpSports.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.SharpSports.dll"]