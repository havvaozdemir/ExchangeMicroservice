FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["ExchangeAPI.csproj", "./"]
RUN dotnet restore "ExchangeAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ExchangeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeAPI.dll"]
