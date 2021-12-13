#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Pudicitia.Identity.Api/Pudicitia.Identity.Api.csproj", "src/Pudicitia.Identity.Api/"]
COPY ["src/Pudicitia.Identity.App/Pudicitia.Identity.App.csproj", "src/Pudicitia.Identity.App/"]
COPY ["src/Pudicitia.Identity.Domain/Pudicitia.Identity.Domain.csproj", "src/Pudicitia.Identity.Domain/"]
COPY ["src/Pudicitia.Common/Pudicitia.Common.csproj", "src/Pudicitia.Common/"]
COPY ["src/Pudicitia.Identity.Data/Pudicitia.Identity.Data.csproj", "src/Pudicitia.Identity.Data/"]
COPY ["src/Pudicitia.Common.EntityFrameworkCore/Pudicitia.Common.EntityFrameworkCore.csproj", "src/Pudicitia.Common.EntityFrameworkCore/"]
COPY ["src/Pudicitia.Common.RabbitMQ/Pudicitia.Common.RabbitMQ.csproj", "src/Pudicitia.Common.RabbitMQ/"]
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
