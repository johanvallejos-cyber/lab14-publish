# Sistema de tickets

API ASP.NET Core en .NET 10. Para ejecutarla localmente, configura la cadena de conexión MySQL mediante la variable de entorno `ConnectionStrings__DefaultConnection` y luego inicia el proyecto `lab11-VallejosJohan`.

## GitHub Actions

El workflow en `.github/workflows/dotnet.yml` se ejecuta al hacer push hacia `main`. Restaura y compila la solución, publica la API y guarda el paquete como artefacto `ticket-api` en la ejecución de Actions.

Para activar el paso de copia al servidor, configura los secretos `SERVER_HOST`, `SERVER_USER`, `SERVER_PATH`, `SERVER_SSH_KEY` y `SERVER_KNOWN_HOSTS` en GitHub. El artefacto publicado no contiene la cadena de conexión; el servidor debe recibirla como `ConnectionStrings__DefaultConnection`.

## Despliegue en Render

En Render crea un Web Service para este repositorio, selecciona la rama `main` y el runtime Docker. El `Dockerfile` está en la raíz. En Environment agrega `ConnectionStrings__DefaultConnection` con una cadena válida hacia una base de datos MySQL accesible desde Render. La API requiere esa cadena para iniciar; guarda la contraseña únicamente en Render, no en el repositorio.
