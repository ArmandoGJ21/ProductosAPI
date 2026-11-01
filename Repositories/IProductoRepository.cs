// =====================================================================
//  IProductoRepository.cs
//  Interface que pide la actividad. Es un "contrato": dice QUÉ
//  operaciones existen, pero no CÓMO se hacen. ProductoRepository es
//  la clase que la implementa con SQL Server.
//
//  Ventaja: los endpoints solo conocen esta interface, así que si
//  mañana se cambia a MySQL, solo se cambia la clase que la implementa.
// =====================================================================

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
