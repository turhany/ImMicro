#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["ImMicro.Api/ImMicro.Api.csproj", "ImMicro.Api/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Resources/ImMicro.Resources.csproj", "ImMicro.Resources/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
RUN dotnet restore "ImMicro.Api/ImMicro.Api.csproj"
COPY . .
RUN dotnet build "ImMicro.Api/ImMicro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImMicro.Api/ImMicro.Api.csproj" -c Release -o /app/publish


FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImMicro.Api.dll"]