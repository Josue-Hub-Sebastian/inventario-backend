🏦 Inventario Banco API
API REST para la gestión de inventario de equipos institucionales.
Desarrollada con .NET 8 siguiendo una arquitectura en capas, con acceso a datos mediante ADO.NET puro y Stored Procedures.

🛠️ Tecnologías
.NET 8 : Framework principal (Web API)
SQL Server : Base de datos relacional
ADO.NET : Acceso a datos sin ORM
ClosedXML : Importacion de archivos Excel
Swagger/OpenAPI : Documentacion Interactiva

🏗️ Arquitectura
Controller → Service → Repository → SQL Server (Stored Procedures)

📊 Importación masiva desde Excel
El endpoint POST /api/Equipo/importar acepta archivos .xlsx con el siguiente formato:

CodigoPatrimonial | TipoEquipo | Marca | Modelo | NumeroSerie | EstadoEquipo | Ubicacion | UsuarioAsignado

Restricciones:
- Máximo 10,000 filas por archivo
- Tamaño máximo: 100 MB
- Validación por fila con reporte de errores detallado

▶️ Ejecución
Dale click a la solucion

🔗 Frontend
Este backend está diseñado para trabajar con un frontend Angular con CORS habilitado.

📄 Licencia
Proyecto de uso personal / institucional.


Agradecimientos a mis ciberamigos
yuls,bri,cielo,oscar,michu,raquel


