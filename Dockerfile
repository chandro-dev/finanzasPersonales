# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia archivos de solución y restaura
COPY *.sln .
COPY Dominio/*.csproj ./Dominio/
COPY Persistencia/*.csproj ./Persistencia/
COPY ApiFinanzas/*.csproj ./ApiFinanzas/

RUN dotnet restore

# Copia todo y construye
COPY . .
RUN dotnet publish ApiFinanzas/ApiFinanzas.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiFinanzas.dll"]
