#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Pudicitia.Identity.Api/Pudicitia.Identity.Api.csproj", "src/Pudicitia.Identity.Api/"]
COPY ["src/Pudicitia.Identity.App/Pudicitia.Identity.App.csproj", "src/Pudicitia.Identity.App/"]
COPY ["src/Pudicitia.Identity.Domain/Pudicitia.Identity.Domain.csproj", "src/Pudicitia.Identity.Domain/"]
COPY ["src/Pudicitia.Common/Pudicitia.Common.csproj", "src/Pudicitia.Common/"]
COPY ["src/Pudicitia.Identity.Data/Pudicitia.Identity.Data.csproj", "src/Pudicitia.Identity.Data/"]
RUN dotnet restore "src/Pudicitia.Identity.Api/Pudicitia.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Pudicitia.Identity.Api"
RUN dotnet build "Pudicitia.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pudicitia.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pudicitia.Identity.Api.dll"]
