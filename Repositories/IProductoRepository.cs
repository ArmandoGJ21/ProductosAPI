

using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

public interface IProductoRepository
{
    // Consultas: devuelve todos los productos
    List<Producto> ObtenerTodos();

    // Buscar: devuelve el producto con ese Id, o null si no existe
    Producto? ObtenerPorId(int id);

    // Insert: guarda el producto y devuelve el Id que generó la base
    int Insertar(Producto producto);

    // Actualizar: devuelve true si lo actualizó, false si no existía
    bool Actualizar(Producto producto);

    // Eliminar: devuelve true si lo borró, false si no existía
    bool Eliminar(int id);
}
