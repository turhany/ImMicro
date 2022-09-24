#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ImMicro.Api/ImMicro.Api.csproj", "ImMicro.Api/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Lock.Redis/ImMicro.Lock.Redis.csproj", "ImMicro.Lock.Redis/"]
COPY ["ImMicro.Lock/ImMicro.Lock.csproj", "ImMicro.Lock/"]
COPY ["ImMicro.Resources/ImMicro.Resources.csproj", "ImMicro.Resources/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
COPY ["ImMicro.Cache/ImMicro.Cache.csproj", "ImMicro.Cache/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
COPY ["ImMicro.Validation.FluentValidation/ImMicro.Validation.FluentValidation.csproj", "ImMicro.Validation.FluentValidation/"]
COPY ["ImMicro.Validation/ImMicro.Validation.csproj", "ImMicro.Validation/"]
COPY ["ImMicro.Cache.Redis/ImMicro.Cache.Redis.csproj", "ImMicro.Cache.Redis/"]
COPY ["ImMicro.Data.EntityFramework/ImMicro.Data.EntityFramework.csproj", "ImMicro.Data.EntityFramework/"]
RUN dotnet restore "ImMicro.Api/ImMicro.Api.csproj"
COPY . .
WORKDIR "/src/ImMicro.Api"
RUN dotnet build "ImMicro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImMicro.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImMicro.Api.dll"]