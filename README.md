# ProductosAPI

Backend en **.NET 10 (Minimal APIs)** con **SQL Server en Docker** para administrar productos (CRUD).
Se prueba con **Swagger**.

## Tecnologías

- .NET 10 — Minimal APIs
- SQL Server 2022 corriendo en **Docker** (`docker-compose.yml`)
- ADO.NET (`Microsoft.Data.SqlClient`) para las consultas
- Swagger (`Swashbuckle.AspNetCore`)

## Estructura del proyecto

```
ProductosAPI/
├── docker-compose.yml                   ← levanta SQL Server y ejecuta el script
├── Database/script.sql                  ← crea la base ProductosDB y la tabla Productos
├── Data/Conexion.cs                     ← Conexión: lee la cadena y crea SqlConnection
├── Models/Producto.cs                   ← la entidad Producto
├── Repositories/
│   ├── IProductoRepository.cs           ← la interface a implementar
│   └── ProductoRepository.cs            ← consultas SQL (SELECT, INSERT, UPDATE, DELETE)
├── Endpoints/ProductosEndpoints.cs      ← rutas /productos que usa Swagger
├── appsettings.json                     ← cadena de conexión al contenedor
└── Program.cs                           ← configuración y registro de servicios
```

## Requisitos

- .NET 10 SDK
- Docker Desktop abierto

## Cómo ejecutarlo

1. **Levantar la base de datos en Docker** (la primera vez descarga la imagen, tarda unos minutos):
   ```bash
   docker compose up -d
   ```
   Esto arranca el contenedor `productos-sqlserver` y, cuando SQL Server está listo,
   el contenedor `productos-db-init` ejecuta `Database/script.sql` y se apaga.

2. **Ejecutar la API:**
   ```bash
   dotnet run
   ```

3. **Abrir Swagger:** http://localhost:5210/swagger

### Comandos útiles de Docker

| Comando | Qué hace |
|---|---|
| `docker compose ps -a` | Ver el estado de los contenedores |
| `docker compose logs db-init` | Ver si el script se ejecutó bien |
| `docker compose down` | Apagar la base (los datos se conservan) |
| `docker compose down -v` | Apagar y **borrar** los datos (la próxima vez se crean de nuevo) |

### Datos de conexión (solo desarrollo)

| Dato | Valor |
|---|---|
| Servidor | `localhost,1433` |
| Usuario | `sa` |
| Contraseña | `Productos#2026` |
| Base de datos | `ProductosDB` |

## Endpoints

| Método | Ruta              | Punto a resolver | Método del repositorio |
|--------|-------------------|------------------|------------------------|
| GET    | `/productos`      | Consultas        | `ObtenerTodos()`       |
| GET    | `/productos/{id}` | Buscar           | `ObtenerPorId(id)`     |
| POST   | `/productos`      | Insert           | `Insertar(producto)`   |
| PUT    | `/productos/{id}` | Actualizar       | `Actualizar(producto)` |
| DELETE | `/productos/{id}` | Eliminar         | `Eliminar(id)`         |

Ejemplo de body para POST / PUT:

```json
{
  "nombre": "Audífonos",
  "descripcion": "Audífonos Bluetooth",
  "precio": 599.99,
  "stock": 15
}
```
