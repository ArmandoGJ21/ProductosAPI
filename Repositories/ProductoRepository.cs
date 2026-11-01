using Microsoft.Data.SqlClient;
using ProductosAPI.Data;
using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

// Implementación de IProductoRepository usando ADO.NET (SqlConnection + SqlCommand)
public class ProductoRepository : IProductoRepository
{
    private readonly Conexion _conexion;

    public ProductoRepository(Conexion conexion)
    {
        _conexion = conexion;
    }

    // Consulta: SELECT de todos los productos
    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();

        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos ORDER BY Id", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            productos.Add(Mapear(reader));
        }

        return productos;
    }

    // Buscar: SELECT de un producto por su Id
    public Producto? ObtenerPorId(int id)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Mapear(reader) : null;
    }

    // Insert: agrega un producto y devuelve el Id que generó SQL Server
    public int Insertar(Producto producto)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand(
            @"INSERT INTO Productos (Nombre, Descripcion, Precio, Stock)
              OUTPUT INSERTED.Id
              VALUES (@Nombre, @Descripcion, @Precio, @Stock)", conn);
        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
        cmd.Parameters.AddWithValue("@Stock", producto.Stock);

        return (int)cmd.ExecuteScalar();
    }

    // Actualizar: modifica todos los campos del producto con ese Id
    public bool Actualizar(Producto producto)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand(
            @"UPDATE Productos
              SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, Stock = @Stock
              WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", producto.Id);
        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
        cmd.Parameters.AddWithValue("@Stock", producto.Stock);

        // Si no se afectó ninguna fila, el producto no existe
        return cmd.ExecuteNonQuery() > 0;
    }

    // Eliminar: borra el producto con ese Id
    public bool Eliminar(int id)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand("DELETE FROM Productos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery() > 0;
    }

    // Convierte la fila actual del reader en un objeto Producto
    private static Producto Mapear(SqlDataReader reader) => new Producto
    {
        Id          = reader.GetInt32(0),
        Nombre      = reader.GetString(1),
        Descripcion = reader.GetString(2),
        Precio      = reader.GetDecimal(3),
        Stock       = reader.GetInt32(4),
    };
}
