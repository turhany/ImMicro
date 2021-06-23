#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ImMicro.Api/ImMicro.Api.csproj", "ImMicro.Api/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
RUN dotnet restore "ImMicro.Api/ImMicro.Api.csproj"
COPY . .
WORKDIR "/src/ImMicro.Api"
RUN dotnet build "ImMicro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImMicro.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImMicro.Api.dll"]
