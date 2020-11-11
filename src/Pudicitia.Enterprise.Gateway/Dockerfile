#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Pudicitia.Enterprise.Gateway/Pudicitia.Enterprise.Gateway.csproj", "src/Pudicitia.Enterprise.Gateway/"]
RUN dotnet restore "src/Pudicitia.Enterprise.Gateway/Pudicitia.Enterprise.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Pudicitia.Enterprise.Gateway"
RUN dotnet build "Pudicitia.Enterprise.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pudicitia.Enterprise.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pudicitia.Enterprise.Gateway.dll"]