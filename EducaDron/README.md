# EducaDron

EducaDron es un proyecto de videojuego educativo desarrollado con Unity. Este documento gu a al tribunal para ejecutar, evaluar y, en su caso, compilar el proyecto. La versi n web p blica est  disponible en:
- https://educadron.itch.io/educadron

## 1. Objetivo del proyecto
EducaDron propone actividades formativas gamificadas en torno al pilotaje y l gica de un dron, integrando UI, progreso y consumo de API para persistencia de datos.

## 2.
### 2.1 Ejecuci n r pida (versi n local Windows)

- Abrir: ".\EducaDron\BuildWindows\EducaDron.exe"
- Requisitos: conexi n estable a Internet. El juego usa un backend p blico.

### 2.2 Ejecuci n r pida (versi n web p blica)

- Abrir: https://educadron.itch.io/educadron
- Requisitos: conexi n estable a Internet. El juego usa un backend p blico.

## 3. Ejecuci n local del build WebGL restringida

El repositorio contiene un build WebGL en `BuildWebGL/`. Debido a restricciones de los navegadores, debe servirse desde un servidor local (no funciona abriendo el `index.html` con doble clic por CORS).

## 4. Compilaci n

### 4.1 Compilar para WebGL
1. Unity > File > Build Settings 
2. Seleccionar  WebGL  y pulsar  Switch Platform .
3. Pulsar  Build  o  Build and Run  y elegir carpeta de salida.
4. Subir el contenido generado a un hosting (por ejemplo, itch.io) o servirlo desde un servidor web.

### 4.2 Compilar para escritorio (Windows)
1. Unity > File > Build Settings 
2. Seleccionar  PC, Mac & Linux Standalone  (arquitectura acorde).
3. Pulsar  Build  y elegir carpeta de salida.
4. Ejecutar el `.exe` generado.

## 5. Configuraci n del backend

El juego consume servicios REST cuya URL base se define en:

- `Assets/Logic/API/ApiConfig.cs`:
  - `ApiConfig.BaseUrl = "https://educadron-api-jfk-bkfcf9ckdqbjfngd.francecentral-01.azurewebsites.net"` (por defecto).
  - M todos de ayuda:
    - `Build(string relativePath)`
    - `BuildWithQuery(string path, params (string key, string value)[] qs)`

Requisitos de red:
- Acceso HTTPS saliente hacia la URL configurada.
- Si el firewall corporativo restringe tr fico, permitir el dominio del backend.

## 6. Estructura relevante del repositorio

- `Assets/Logic/API/`:
  - `ApiConfig.cs`: configuraci n de URL base y helpers.
  - Otras clases de acceso a API (login, puntos, logout).
- `Assets/Logic/UI/`: l gica del men  principal y UI.
- `BuildWebGL/`: build web listo para servir con `index.html`.
- `Assets/Plugins/Demigiant/DOTween/`: tweening.
- `Assets/RainMaker/`: efectos de lluvia.

## 9. Controles y experiencia de usuario

- La interfaz gu a al usuario a trav s del men  principal y las actividades.
- Los detalles de control espec ficos se presentan en el propio juego seg n la escena/actividad.

## 10. Soluci n de problemas

- No carga en navegador (WebGL):
  - Servir bajo HTTP/HTTPS (no abrir `index.html` directamente).
  - Probar con otro navegador o limpiar cach .
- Errores de API / autenticaci n:
  - Tanto la API como la base de datos se aloja en un servidor de Azure modalidad: "serverless" por lo cual se inactiva tras 1 hora sin solicitudes.
  Puede que el primer request no obtenga respuesta, espere 30-60 segundos a que la Base de Datos reinicie su actividad.
  - Verificar conectividad a `ApiConfig.BaseUrl`.
  - Revisar que el backend est  operativo:
  - Puede acceder al Swagger de la API y ejecutar el endpoint /health. Response esperada: "OK": "https://educadron-api-jfk-bkfcf9ckdqbjfngd.francecentral-01.azurewebsites.net/swagger/index.html"

## 12. Autor a y contacto

- Equipo de desarrollo:
  - Kniazev, Juan.
  - Nande, Serrana.
  - Sosa, Joaquín.

- Contacto: educadronUDE@gmail.com