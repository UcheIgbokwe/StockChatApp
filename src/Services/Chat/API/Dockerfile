#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Chat/API/API.csproj", "src/Services/Chat/API/"]
COPY ["src/Services/Chat/Infrastructure/Infrastructure.csproj", "src/Services/Chat/Infrastructure/"]
COPY ["src/Services/Chat/Domain/Domain.csproj", "src/Services/Chat/Domain/"]
COPY ["src/BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "src/BuildingBlocks/EventBus.Messages/"]

RUN dotnet restore "src/Services/Chat/API/API.csproj"
COPY . .
WORKDIR "/src/src/Services/Chat/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
