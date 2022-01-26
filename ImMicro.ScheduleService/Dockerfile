#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ImMicro.ScheduleService/ImMicro.ScheduleService.csproj", "ImMicro.ScheduleService/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Resources/ImMicro.Resources.csproj", "ImMicro.Resources/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
RUN dotnet restore "ImMicro.ScheduleService/ImMicro.ScheduleService.csproj"
COPY . .
WORKDIR "/src/ImMicro.ScheduleService"
RUN dotnet build "ImMicro.ScheduleService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImMicro.ScheduleService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImMicro.ScheduleService.dll"]