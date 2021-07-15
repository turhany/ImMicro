FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ImMicro.ScheduleService/ImMicro.ScheduleService.csproj", "ImMicro.ScheduleService/"]
COPY ["ImMicro.Common/ImMicro.Common.csproj", "ImMicro.Common/"]
COPY ["ImMicro.Contract/ImMicro.Contract.csproj", "ImMicro.Contract/"]
COPY ["ImMicro.Container/ImMicro.Container.csproj", "ImMicro.Container/"]
COPY ["ImMicro.Data/ImMicro.Data.csproj", "ImMicro.Data/"]
COPY ["ImMicro.Model/ImMicro.Model.csproj", "ImMicro.Model/"]
COPY ["ImMicro.Business/ImMicro.Business.csproj", "ImMicro.Business/"]
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