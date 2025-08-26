FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SmartPlantCareApi.csproj", "./"]
RUN dotnet restore "SmartPlantCareApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SmartPlantCareApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartPlantCareApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartPlantCareApi.dll"] 