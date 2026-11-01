// =====================================================================
//  ProductosEndpoints.cs
//  Define las rutas HTTP (/productos) que se prueban en Swagger.
//  Cada endpoint:
//    1. recibe la petición (y el JSON si lo hay),
//    2. valida los datos,
//    3. llama al repositorio,
//    4. responde con el código HTTP correcto.
//  Los endpoints NO escriben SQL: eso solo lo hace el repositorio.
// =====================================================================

using ProductosAPI.Models;
using ProductosAPI.Repositories;

namespace ProductosAPI.Endpoints;

public static class ProductosEndpoints
{
    // Se llama desde Program.cs para registrar todas las rutas
    public static void Map(WebApplication app)
    {
        // MapGroup: todas las rutas de abajo empiezan con /productos.
        // WithTags agrupa los endpoints bajo "Productos" en Swagger.
        var group = app.MapGroup("/productos").WithTags("Productos");

        group.MapGet("/", ObtenerTodos);        // GET    /productos
        group.MapGet("/{id}", ObtenerPorId);    // GET    /productos/5
        group.MapPost("/", Crear);              // POST   /productos
        group.MapPut("/{id}", Actualizar);      // PUT    /productos/5
        group.MapDelete("/{id}", Eliminar);     // DELETE /productos/5
    }

    // ---------------------------------------------------------------
    // Nota sobre los parámetros de cada método:
    //  - int id                  -> sale de la URL ({id})
    //  - Producto producto       -> sale del JSON del body
    //  - IProductoRepository repo -> lo entrega .NET (inyección de
    //                               dependencias registrada en Program.cs)
    // ---------------------------------------------------------------

    // GET /productos -> Consultas
    // Responde 200 OK con la lista de productos
    static IResult ObtenerTodos(IProductoRepository repo)
        => Results.Ok(repo.ObtenerTodos());

    // GET /productos/{id} -> Buscar
    // 200 OK con el producto, o 404 Not Found si no existe
    static IResult ObtenerPorId(int id, IProductoRepository repo)
    {
        var producto = repo.ObtenerPorId(id);
        return producto == null
            ? Results.NotFound(new { mensaje = "Producto no encontrado" })
            : Results.Ok(producto);
    }

    // POST /productos -> Insert
    static IResult Crear(Producto producto, IProductoRepository repo)
    {
        // 1) Validar antes de tocar la base de datos
        var error = Validar(producto);
        if (error != null)
            return Results.BadRequest(new { error });

        // 2) Guardar y asignar el Id que generó SQL Server
        producto.Id = repo.Insertar(producto);

        // 3) 201 + la URL donde se puede consultar el producto nuevo
        return Results.Created($"/productos/{producto.Id}", producto);
    }

    // PUT /productos/{id} -> Actualizar
    static IResult Actualizar(int id, Producto producto, IProductoRepository repo)
    {
        var error = Validar(producto);
        if (error != null)
            return Results.BadRequest(new { error });

        // El Id se toma de la URL, no del JSON
        producto.Id = id;

        return repo.Actualizar(producto)
            ? Results.NoContent()
            : Results.NotFound(new { mensaje = "Producto no encontrado" });
    }

    // DELETE /productos/{id} -> Eliminar
    static IResult Eliminar(int id, IProductoRepository repo)
    {
        return repo.Eliminar(id)
            ? Results.NoContent()
            : Results.NotFound(new { mensaje = "Producto no encontrado" });
    }

    // Validaciones básicas. Regresa el mensaje de error, o null si todo está bien. Nunca se debe confiar en que el cliente manda datos correctos.
    static string? Validar(Producto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
            return "El nombre es obligatorio";
        if (producto.Nombre.Length > 100)
            return "El nombre no puede superar 100 caracteres";
        if (producto.Precio < 0)
            return "El precio no puede ser negativo";
        if (producto.Stock < 0)
            return "El stock no puede ser negativo";
        return null;
    }
}
