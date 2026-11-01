using ProductosAPI.Models;
using ProductosAPI.Repositories;

namespace ProductosAPI.Endpoints;

public static class ProductosEndpoints
{
    // Registra todos los endpoints de /productos
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/productos").WithTags("Productos");

        group.MapGet("/", ObtenerTodos);
        group.MapGet("/{id}", ObtenerPorId);
        group.MapPost("/", Crear);
        group.MapPut("/{id}", Actualizar);
        group.MapDelete("/{id}", Eliminar);
    }

    // GET /productos
    static IResult ObtenerTodos(IProductoRepository repo)
        => Results.Ok(repo.ObtenerTodos());

    // GET /productos/1
    static IResult ObtenerPorId(int id, IProductoRepository repo)
    {
        var producto = repo.ObtenerPorId(id);
        return producto == null
            ? Results.NotFound(new { mensaje = "Producto no encontrado" })
            : Results.Ok(producto);
    }

    // POST /productos
    static IResult Crear(Producto producto, IProductoRepository repo)
    {
        var error = Validar(producto);
        if (error != null)
            return Results.BadRequest(new { error });

        producto.Id = repo.Insertar(producto);
        return Results.Created($"/productos/{producto.Id}", producto);
    }

    // PUT /productos/1
    static IResult Actualizar(int id, Producto producto, IProductoRepository repo)
    {
        var error = Validar(producto);
        if (error != null)
            return Results.BadRequest(new { error });

        producto.Id = id;
        return repo.Actualizar(producto)
            ? Results.NoContent()
            : Results.NotFound(new { mensaje = "Producto no encontrado" });
    }

    // DELETE /productos/1
    static IResult Eliminar(int id, IProductoRepository repo)
    {
        return repo.Eliminar(id)
            ? Results.NoContent()
            : Results.NotFound(new { mensaje = "Producto no encontrado" });
    }

    // Validaciones básicas antes de guardar
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
