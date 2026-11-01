# ProductosAPI

Backend en **.NET 10 (Minimal APIs)** con **SQL Server** para administrar productos (CRUD).
Se prueba con **Swagger**.

## Tecnologías

- .NET 10 — Minimal APIs
- SQL Server (LocalDB) con ADO.NET (`Microsoft.Data.SqlClient`)
- Swagger (`Swashbuckle.AspNetCore`)

## Estructura del proyecto

```
ProductosAPI/
├── Database/script.sql                  ← crea la base ProductosDB y la tabla Productos
├── Data/Conexion.cs                     ← Conexión: lee la cadena y crea SqlConnection
├── Models/Producto.cs                   ← la entidad Producto
├── Repositories/
│   ├── IProductoRepository.cs           ← la interface a implementar
│   └── ProductoRepository.cs            ← consultas SQL (SELECT, INSERT, UPDATE, DELETE)
├── Endpoints/ProductosEndpoints.cs      ← rutas /productos que usa Swagger
├── appsettings.json                     ← cadena de conexión
└── Program.cs                           ← configuración y registro de servicios
```

## Cómo ejecutarlo

1. Crear la base de datos:
   ```bash
   sqlcmd -S "(localdb)\MSSQLLocalDB" -i Database/script.sql
   ```
   (También se puede abrir `Database/script.sql` en SQL Server Management Studio y ejecutarlo.)
2. Ejecutar la API:
   ```bash
   dotnet run
   ```
3. Abrir Swagger: http://localhost:5210/swagger

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

## Flujo del proyecto

<!-- TODO: describe aquí el flujo con tus propias palabras -->
