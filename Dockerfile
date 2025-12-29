# 1. Build faza
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiramo SVE fajlove iz projekta (Application, Modules, sln, props...)
COPY . .

# Radimo restore i publish
# Pazi na putanju: Application/Application.csproj
RUN dotnet restore "Application/Application.csproj"
RUN dotnet publish "Application/Application.csproj" -c Release -o /app/publish

# 2. Run faza
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Application.dll"]