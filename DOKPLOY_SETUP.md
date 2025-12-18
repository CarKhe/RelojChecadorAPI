# Guía de Despliegue en Dokploy

## 📋 Requisitos previos

- ✅ Cuenta en Dokploy
- ✅ Repositorio en GitHub (ya lo tienes: CarKhe/RelojChecadorAPI)
- ✅ Token de acceso a GitHub (opcional pero recomendado)

---

## 🚀 Pasos de Despliegue

### PASO 1: Acceder a Dokploy

1. Abre tu panel de Dokploy en el navegador
2. Inicia sesión con tus credenciales

### PASO 2: Crear un Nuevo Proyecto

1. Haz clic en **"Crear Proyecto"** o **"New Project"**
2. Asigna un nombre: `reloj-checador` (o el que prefieras)
3. Haz clic en **"Crear"**

### PASO 3: Agregar una Aplicación Docker

1. Dentro del proyecto, haz clic en **"Agregar Aplicación"** o **"Add Application"**
2. Selecciona **"Docker"** como tipo de aplicación
3. Elige **"GitHub Repository"** como fuente
4. Haz clic en **"Conectar GitHub"** (si es la primera vez)

### PASO 4: Seleccionar el Repositorio

1. Busca o selecciona: **CarKhe/RelojChecadorAPI**
2. Selecciona rama: **main**
3. Haz clic en **"Siguiente"** o **"Next"**

### PASO 5: Configurar Variables de Entorno

En la sección de **"Environment Variables"** o **"Configuración"**, agrega estas variables:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
ConnectionStrings__DefaultConnection=Server=relojchecador-dbrelojchecador-lkwo1x;Port=3306;Database=mysql;Uid=mysql;Pwd=rg4bjhszrgdruisa;
```

**Pasos:**
1. Haz clic en **"Agregar Variable"**
2. Clave: `ASPNETCORE_ENVIRONMENT`
3. Valor: `Production`
4. Repite para las otras variables

### PASO 6: Configurar Puerto

1. En la sección **"Ports"** o **"Puertos"**
2. Puerto Interno: `8080`
3. Puerto Externo: `9000` (o el que desees)
4. Protocolo: `HTTP`

### PASO 7: Configurar Dockerfile

1. En **"Dockerfile Path"** o **"Ruta del Dockerfile"**, ingresa: `Dockerfile`
2. En **"Build Context"** (si aparece), deja: `.` (punto)

### PASO 8: Configurar Redeploy Automático (Opcional)

1. Busca la opción **"Auto Deploy"** o **"Redeploy Automático"**
2. Habilítalo para que se redepliegue automáticamente al hacer push a main

### PASO 9: Revisar y Desplegar

1. Revisa toda la configuración
2. Haz clic en **"Desplegar"** o **"Deploy"**
3. Espera a que Dokploy:
   - Clone el repositorio
   - Construya la imagen Docker
   - Ejecute el contenedor

### PASO 10: Verificar el Despliegue

1. Ve a la pestaña **"Logs"** o **"Registros"**
2. Busca el mensaje: **"Now listening on: http://[::]:8080"**
3. Tu aplicación debería estar disponible en: `http://tu-servidor:9000`

---

## 🔗 URLs de Acceso

Una vez desplegado:

| Componente | URL |
|-----------|-----|
| API Base | `http://tu-servidor-dokploy:9000` |
| Swagger UI | `http://tu-servidor-dokploy:9000/swagger` |

---

## ⚠️ Posibles Problemas

### Problema: "Failed to connect to database"

**Solución:**
- Verifica que la cadena de conexión sea correcta
- Comprueba que tu servidor Dokploy pueda conectar a MySQL remoto
- Asegúrate de que el firewall del servidor MySQL permite la conexión

### Problema: "Cannot pull image"

**Solución:**
- Verifica que el Dockerfile existe en el repositorio
- Comprueba que GitHub está conectado correctamente a Dokploy

### Problema: "Application keeps restarting"

**Solución:**
- Revisa los logs en Dokploy
- Verifica que todas las variables de entorno están configuradas
- Asegúrate de que la base de datos es accesible

### Problema: "Port already in use"

**Solución:**
- Cambia el puerto externo a otro disponible (ej: 3000, 5001, 8000)

---

## 📊 Monitoreo

Una vez desplegado en Dokploy:

1. **Logs en vivo:** Dokploy muestra los logs en tiempo real
2. **Estado:** Verde = corriendo, Rojo = error
3. **Reiniciar:** Botón "Restart" o "Reiniciar"
4. **Redeploy:** Botón "Redeploy" después de hacer push

---

## 🔄 Actualizar la Aplicación

Después de hacer cambios:

```bash
# En tu máquina local
git add -A
git commit -m "Cambios"
git push origin main
```

Dokploy **automáticamente** (si está habilitado):
- Detectará el nuevo push
- Construirá la imagen
- Desplegará la versión actualizada

---

## 💡 Tips Útiles

✅ **Naming:** Usa nombres descriptivos para tus proyectos y aplicaciones  
✅ **Backups:** Ten backups de tu base de datos MySQL  
✅ **Monitor:** Revisa regularmente los logs para detectar problemas  
✅ **Secrets:** No pongas contraseñas en el Dockerfile, usa variables de entorno  

---

## 📞 Soporte

Si tienes problemas:

1. Revisa los **logs en Dokploy**
2. Verifica la **conectividad de red** a MySQL
3. Comprueba que el **Dockerfile** está correcto
4. Consulta la documentación de Dokploy

---

¿Necesitas ayuda con algún paso específico?
