#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ImMicro.Consumer/ImMicro.Consumer.csproj", "ImMicro.Consumer/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Resources/ImMicro.Resources.csproj", "ImMicro.Resources/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
RUN dotnet restore "ImMicro.Consumer/ImMicro.Consumer.csproj"
COPY . .
WORKDIR "/src/ImMicro.Consumer"
RUN dotnet build "ImMicro.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImMicro.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImMicro.Consumer.dll"]