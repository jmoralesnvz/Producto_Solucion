*- Instrucciones para ejecutar el proyecto.* 

    dotnet run --project ProductosApi

para entrar al swagger

    localhost:port/api/swagget/intex.html

 *-	Configuración de SQLite.* 
SQLite se encuentra configurado solo se requiere ejecutar los comandos de migración
	
 *- Comandos para ejecutar las migraciones.* 

    dotnet ef migrations add InitialCreate
    dotnet ef database update

 *- Ejemplos de peticiones API.* 

    curl -X GET "http://localhost:7170/api/Product?page=1&pageSize=10&search=laptop" -H "Accept: application/json"
    
    fetch("http://localhost:7170/api/Product?page=1&pageSize=10&search=laptop")
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error:", error));


 *- Pasos para ejecutar la aplicación MVC y API.*
 ejecuta:

     dotnet restore

para iniciar la API si no la has iniciado

    dotnet run --project ProductosApi

Esto levantará la API en http://localhost:7170 (o el puerto configurado).
Puedes verificar que la API funciona accediendo a:

-   http://localhost:7170/swagger → Para ver la documentación de Swagger.
-   http://localhost:7170/api/Product → Para ver la lista de productos en JSON.
*Configurar el Cliente MVC*

    dotnet restore
       {
          "ApiUrl": "http://localhost:7170/api/Product"
        }

 *Iniciar la aplicación MVC*
Ejecuta el siguiente comando en la terminal dentro del proyecto ProductosMvc:
dotnet run --project ProductosMvc 
La aplicación estará disponible en http://localhost:5000 (o el puerto configurado).
