#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Pudicitia.HR.Api/Pudicitia.HR.Api.csproj", "src/Pudicitia.HR.Api/"]
RUN dotnet restore "src/Pudicitia.HR.Api/Pudicitia.HR.Api.csproj"
COPY . .
WORKDIR "/src/src/Pudicitia.HR.Api"
RUN dotnet build "Pudicitia.HR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pudicitia.HR.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pudicitia.HR.Api.dll"]