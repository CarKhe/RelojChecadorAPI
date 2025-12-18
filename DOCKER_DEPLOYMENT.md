# Guía de Deploymente en Docker y Dokploy

## Descripción General

Este proyecto es una API REST en ASP.NET Core 9.0 que usa MySQL. Está completamente dockerizado y listo para ser desplegado en Dokploy.

## Archivos Creados

1. **Dockerfile** - Imagen multi-stage para compilar y ejecutar la API
2. **docker-compose.yml** - Configuración para ejecutar localmente con MySQL
3. **.dockerignore** - Archivos que se ignoran en la construcción
4. **appsettings.Production.json** - Configuración para producción con variables de entorno

## Ejecución Local con Docker

### Requisitos
- Docker instalado
- Docker Compose instalado

### Pasos

1. **Navega al directorio del proyecto:**
   ```bash
   cd /Users/carlosrodriguez/Documents/proyectos/relojChecador/relojChecadorAPI
   ```

2. **Construir y ejecutar con docker-compose:**
   ```bash
   docker-compose up --build
   ```

3. **Acceder a la API:**
   - API: http://localhost:5000
   - Swagger: http://localhost:5000/swagger

4. **Detener los contenedores:**
   ```bash
   docker-compose down
   ```

## Despliegue en Dokploy

### Pasos para desplegar

#### 1. Preparar el repositorio
Asegúrate de que todos los archivos (incluyendo el Dockerfile) estén en tu repositorio Git:

```bash
git add Dockerfile docker-compose.yml .dockerignore appsettings.Production.json DOCKER_DEPLOYMENT.md
git commit -m "Add Docker configuration for Dokploy deployment"
git push
```

#### 2. En la interfaz de Dokploy

1. **Crear un nuevo proyecto**
   - Ve a tu panel de Dokploy
   - Crea un nuevo proyecto

2. **Agregar una aplicación Docker**
   - Selecciona "Aplicación" → "Docker"
   - Conecta tu repositorio Git
   - Selecciona la rama `main`

3. **Configurar variables de entorno**
   En el panel de configuración, agrega las siguientes variables:

   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   DB_HOST=mysql-container
   DB_PORT=3306
   DB_NAME=db_RelojChecador
   DB_USER=relojuser
   DB_PASSWORD=TuContraseñaSegura
   JWT_KEY=ys7gpnSA/G2jf8yWe2Xr+VDb+N0sXETTyY8FV9XGrHs=
   JWT_ISSUER=AsistenciaAPI
   JWT_AUDIENCE=AsistenciaAngularApp
   ```

4. **Configurar puerto**
   - Puerto interno: 8080
   - Puerto externo: selecciona un puerto disponible (ej: 3000, 5000)

5. **Agregar base de datos MySQL (opcional en Dokploy)**
   Si Dokploy es compatible con servicios de bases de datos:
   - Crea un servicio MySQL
   - Conecta la API con la base de datos

#### 3. Desplegar
- Haz clic en "Desplegar"
- Dokploy construirá la imagen y ejecutará el contenedor

### Alternativa: Desplegar manualmente en un servidor

Si necesitas desplegar en tu propio servidor:

```bash
# 1. Clona el repositorio en el servidor
git clone <tu-repositorio>
cd relojChecadorAPI

# 2. Construir la imagen Docker
docker build -t reloj-checador-api:latest .

# 3. Ejecutar con docker-compose (que incluye MySQL)
docker-compose up -d

# 4. Verificar que todo está corriendo
docker ps
docker logs reloj-checador-api
```

## Problemas Comunes

### La API no puede conectar a la base de datos

**Problema:** `Connection refused` o timeout

**Solución:**
- Verifica que MySQL esté corriendo: `docker ps`
- Revisa los logs: `docker logs mysql`
- Espera a que MySQL inicie completamente (puede tardar 20-30 segundos)
- Asegúrate de usar el nombre del servicio como host (`mysql` en docker-compose)

### Error: `JWT Key not found`

**Problema:** La variable `Jwt__Key` no está configurada

**Solución:**
- En Dokploy, asegúrate de agregar `JWT_KEY` en las variables de entorno
- Usa el mismo valor de la configuración de desarrollo

### Puerto ya en uso

**Problema:** El puerto 5000 ya está en uso

**Solución:**
```bash
# Cambiar puerto en docker-compose.yml o en Dokploy
# Cambiar la primera línea de "ports" de:
# - "5000:8080"
# a:
# - "5001:8080"
```

## Variables de Entorno para Producción

| Variable | Descripción | Valor por defecto |
|----------|-------------|-------------------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente | Production |
| `ASPNETCORE_URLS` | URL de escucha | http://+:8080 |
| `DB_HOST` | Host de MySQL | mysql |
| `DB_PORT` | Puerto de MySQL | 3306 |
| `DB_NAME` | Nombre de BD | db_RelojChecador |
| `DB_USER` | Usuario de BD | relojuser |
| `DB_PASSWORD` | Contraseña de BD | (requerida) |
| `JWT_KEY` | Clave JWT | (requerida) |
| `JWT_ISSUER` | Emisor JWT | AsistenciaAPI |
| `JWT_AUDIENCE` | Audiencia JWT | AsistenciaAngularApp |

## Verificar el despliegue

Una vez desplegado, verifica que todo funcione:

```bash
# Ver logs
docker logs reloj-checador-api

# Hacer una solicitud HTTP
curl http://localhost:5000/swagger

# Comprobar salud de la API (ajusta el endpoint según tu API)
curl http://localhost:5000/api/health
```

## Monitoreo y Mantenimiento

### Ver logs en tiempo real
```bash
docker logs -f reloj-checador-api
```

### Reiniciar la aplicación
```bash
docker-compose restart api
```

### Actualizar a la última versión
```bash
git pull
docker-compose up --build -d
```

### Limpiar recursos de Docker
```bash
docker-compose down -v
```

## Recursos útiles

- [Documentación de Docker](https://docs.docker.com/)
- [Documentación de Dokploy](https://dokploy.com/)
- [ASP.NET Core en Contenedores](https://learn.microsoft.com/en-us/dotnet/architecture/containerized-lifecycle/)

---

¿Necesitas ayuda con algún paso específico?
