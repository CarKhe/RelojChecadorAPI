# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiar archivos de proyecto
COPY ["relojChecadorAPI.csproj", "./"]

# Restaurar dependencias
RUN dotnet restore "relojChecadorAPI.csproj"

# Copiar el código fuente
COPY . .

# Compilar la aplicación
RUN dotnet build "relojChecadorAPI.csproj" -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish

RUN dotnet publish "relojChecadorAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Copiar la aplicación publicada desde la etapa de publish
COPY --from=publish /app/publish .

# Exponer el puerto
EXPOSE 8080

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "relojChecadorAPI.dll"]
