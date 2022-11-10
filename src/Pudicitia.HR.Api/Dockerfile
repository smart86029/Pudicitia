#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pudicitia.HR.Api/Pudicitia.HR.Api.csproj", "src/Pudicitia.HR.Api/"]
COPY ["src/Pudicitia.HR.App/Pudicitia.HR.App.csproj", "src/Pudicitia.HR.App/"]
COPY ["src/Pudicitia.HR.Data/Pudicitia.HR.Data.csproj", "src/Pudicitia.HR.Data/"]
COPY ["src/Pudicitia.HR.Domain/Pudicitia.HR.Domain.csproj", "src/Pudicitia.HR.Domain/"]
COPY ["src/Pudicitia.HR.Events/Pudicitia.HR.Events.csproj", "src/Pudicitia.HR.Events/"]
COPY ["src/Pudicitia.Common/Pudicitia.Common.csproj", "src/Pudicitia.Common/"]
COPY ["src/Pudicitia.Common.Dapper/Pudicitia.Common.Dapper.csproj", "src/Pudicitia.Common.Dapper/"]
COPY ["src/Pudicitia.Common.EntityFrameworkCore/Pudicitia.Common.EntityFrameworkCore.csproj", "src/Pudicitia.Common.EntityFrameworkCore/"]
COPY ["src/Pudicitia.Common.RabbitMQ/Pudicitia.Common.RabbitMQ.csproj", "src/Pudicitia.Common.RabbitMQ/"]
RUN dotnet restore "src/Pudicitia.HR.Api/Pudicitia.HR.Api.csproj"
COPY . .
WORKDIR "/src/src/Pudicitia.HR.Api"
RUN dotnet build "Pudicitia.HR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pudicitia.HR.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pudicitia.HR.Api.dll"]
