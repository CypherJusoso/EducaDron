# EducaDron

EducaDron es un proyecto de videojuego educativo desarrollado con Unity. Este documento guía al tribunal para ejecutar, evaluar y, en su caso, compilar el proyecto. La versión web pública está disponible en:
- https://educadron.itch.io/educadron

## 1. Objetivo del proyecto
EducaDron propone actividades formativas gamificadas en torno al pilotaje y lógica de un dron, integrando UI, progreso y consumo de API para persistencia de datos.

## 2. Requisitos

- Unity (versión exacta del proyecto): consultar `ProjectSettings/ProjectVersion.txt`.
  - Recomendado: instalar esa versión vía Unity Hub (LTS equivalente o superior cuando sea posible).
- Sistema operativo: Windows 10/11 o equivalente compatible con la versión de Unity indicada.
- Conexión a Internet para acceso al backend.
- Navegador moderno (para WebGL): Chrome, Edge o Firefox actualizados.

Dependencias de terceros incluidas en `Assets`:
- DOTween (Demigiant)
- Rain Maker (Digital Ruby)

## 3. Ejecución rápida (versión web pública)

- Abrir: https://educadron.itch.io/educadron
- Requisitos: conexión estable a Internet. El juego usa un backend público.

## 4. Ejecución local con Unity Editor

1. Abrir Unity Hub.
2. Añadir la carpeta del proyecto y abrir con la versión de Unity indicada en `ProjectVersion.txt`.
3. Cargar la escena principal (si no se abre por defecto).
4. Pulsar Play en el Editor.

Notas:
- El juego consume un backend configurado en tiempo de ejecución (ver “Configuración del backend”).
- Para pruebas sin bloquear, mantener conexión a la URL del API.

## 5. Ejecución local del build WebGL (incluido)

El repositorio contiene un build WebGL en `BuildWebGL/`. Debido a restricciones de los navegadores, debe servirse desde un servidor local (no funciona abriendo el `index.html` con doble clic por CORS).

- Usando Python 3:
  1) Abrir terminal en la carpeta que contiene `BuildWebGL\index.html`.
  2) Ejecutar:
     - Windows: `py -m http.server 8080`
     - Otras plataformas: `python3 -m http.server 8080`
  3) Navegar a: http://localhost:8080/BuildWebGL/index.html

## 6. Compilación

### 6.1 Compilar para WebGL
1. Unity > File > Build Settings…
2. Seleccionar “WebGL” y pulsar “Switch Platform”.
3. Pulsar “Build” o “Build and Run” y elegir carpeta de salida.
4. Subir el contenido generado a un hosting (por ejemplo, itch.io) o servirlo desde un servidor web.

### 6.2 Compilar para escritorio (Windows)
1. Unity > File > Build Settings…
2. Seleccionar “PC, Mac & Linux Standalone” (arquitectura acorde).
3. Pulsar “Build” y elegir carpeta de salida.
4. Ejecutar el `.exe` generado.

## 7. Configuración del backend

El juego consume servicios REST cuya URL base se define en:

- `Assets/Logic/API/ApiConfig.cs`:
  - `ApiConfig.BaseUrl = "https://educadron-api-jfk-bkfcf9ckdqbjfngd.francecentral-01.azurewebsites.net"` (por defecto).
  - Métodos de ayuda:
    - `Build(string relativePath)`
    - `BuildWithQuery(string path, params (string key, string value)[] qs)`

Para apuntar a otro entorno (p. ej. staging/producción), modificar `BaseUrl` y reconstruir.

Requisitos de red:
- Acceso HTTPS saliente hacia la URL configurada.
- Si el firewall corporativo restringe tráfico, permitir el dominio del backend.

## 8. Estructura relevante del repositorio

- `Assets/Logic/API/`:
  - `ApiConfig.cs`: configuración de URL base y helpers.
  - Otras clases de acceso a API (login, puntos, logout).
- `Assets/Logic/UI/`: lógica del menú principal y UI.
- `BuildWebGL/`: build web listo para servir con `index.html`.
- `Assets/Plugins/Demigiant/DOTween/`: tweening.
- `Assets/RainMaker/`: efectos de lluvia.

## 9. Controles y experiencia de usuario

- La interfaz guía al usuario a través del menú principal y las actividades.
- Los detalles de control específicos se presentan en el propio juego según la escena/actividad.

## 10. Solución de problemas

- No carga en navegador (WebGL):
  - Servir bajo HTTP/HTTPS (no abrir `index.html` directamente).
  - Probar con otro navegador o limpiar caché.
- Errores de API / autenticación:
  - Verificar conectividad a `ApiConfig.BaseUrl`.
  - Revisar que el backend esté operativo.
- Incompatibilidad de versión de Unity:
  - Instalar la versión exacta indicada en `ProjectVersion.txt` vía Unity Hub.

## 11. Licencias de terceros

- DOTween © Demigiant — ver `Assets/Plugins/Demigiant/DOTween/readme.txt` y su licencia.
- Rain Maker © Digital Ruby — ver `Assets/RainMaker/Readme.txt`.

## 12. Autoría y contacto

- Equipo de desarrollo: EducaDron.
- Contacto: indicar correo o canal oficial del equipo si procede.
