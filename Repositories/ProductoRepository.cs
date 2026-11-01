// =====================================================================
//  ProductoRepository.cs
//  Implementa IProductoRepository. Es la ÚNICA clase que habla con la
//  base de datos. Usa ADO.NET:
//    - SqlConnection -> la conexión a SQL Server
//    - SqlCommand    -> la instrucción SQL que se va a ejecutar
//    - SqlDataReader -> recorre las filas que regresa un SELECT
//
//  Todas las consultas usan parámetros (@Id, @Nombre, ...) en lugar de
//  pegar los valores en el texto SQL. Así se evita la inyección SQL.
// =====================================================================

using Microsoft.Data.SqlClient;
using ProductosAPI.Data;
using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

public class ProductoRepository : IProductoRepository
{
    // Objeto que fabrica las conexiones (ver Data/Conexion.cs)
    private readonly Conexion _conexion;

    // .NET entrega la Conexion automáticamente (inyección de dependencias,
    // registrada en Program.cs)
    public ProductoRepository(Conexion conexion)
    {
        _conexion = conexion;
    }

    // -----------------------------------------------------------------
    // PUNTO A RESOLVER: Consultas
    // SELECT de todos los productos
    // -----------------------------------------------------------------
    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();

        // 1) Crear la conexión y abrirla.
        //    "using" la cierra automáticamente al salir del método.
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        // 2) Preparar la instrucción SQL
        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos ORDER BY Id", conn);

        // 3) Ejecutarla. ExecuteReader se usa cuando el SQL regresa filas.
        using var reader = cmd.ExecuteReader();

        // 4) Read() avanza fila por fila; regresa false cuando ya no hay más
        while (reader.Read())
        {
            // 5) Convertir la fila en un objeto Producto y agregarlo a la lista
            productos.Add(Mapear(reader));
        }

        return productos;
    }

    // -----------------------------------------------------------------
    // PUNTO A RESOLVER: Buscar
    // SELECT de un solo producto por su Id
    // -----------------------------------------------------------------
    public Producto? ObtenerPorId(int id)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        // @Id es un parámetro: su valor se manda por separado (línea de abajo)
        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        // Si Read() encuentra una fila, la convierte en Producto.
        // Si no hay filas, el producto no existe y se regresa null.
        return reader.Read() ? Mapear(reader) : null;
    }

    // -----------------------------------------------------------------
    // PUNTO A RESOLVER: Insert
    // Agrega un producto y devuelve el Id que generó SQL Server
    // -----------------------------------------------------------------
    public int Insertar(Producto producto)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        // OUTPUT INSERTED.Id hace que SQL Server regrese el Id nuevo
        // (el que generó IDENTITY) en la misma instrucción.
        using var cmd = new SqlCommand(
            @"INSERT INTO Productos (Nombre, Descripcion, Precio, Stock)
              OUTPUT INSERTED.Id
              VALUES (@Nombre, @Descripcion, @Precio, @Stock)", conn);

        // Pasar cada valor del producto a su parámetro
        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
        cmd.Parameters.AddWithValue("@Stock", producto.Stock);

        // ExecuteScalar se usa cuando el SQL regresa UN solo valor
        // (aquí, el Id nuevo).
        return (int)cmd.ExecuteScalar();
    }

    // -----------------------------------------------------------------
    // PUNTO A RESOLVER: Actualizar
    // Modifica todos los campos del producto con ese Id
    // -----------------------------------------------------------------
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

        // ExecuteNonQuery se usa para INSERT/UPDATE/DELETE y regresa
        // cuántas filas se modificaron. Si es 0, no existía ese Id.
        return cmd.ExecuteNonQuery() > 0;
    }

    // -----------------------------------------------------------------
    // PUNTO A RESOLVER: Eliminar
    // Borra el producto con ese Id
    // -----------------------------------------------------------------
    public bool Eliminar(int id)
    {
        using var conn = _conexion.ObtenerConexion();
        conn.Open();

        using var cmd = new SqlCommand("DELETE FROM Productos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        // true si se borró al menos una fila; false si el Id no existía
        return cmd.ExecuteNonQuery() > 0;
    }

    // -----------------------------------------------------------------
    // Método auxiliar: convierte la fila actual del reader en Producto.
    // Los números (0, 1, 2...) son la posición de cada columna en el
    // SELECT: Id=0, Nombre=1, Descripcion=2, Precio=3, Stock=4.
    // -----------------------------------------------------------------
    private static Producto Mapear(SqlDataReader reader) => new Producto
    {
        Id          = reader.GetInt32(0),
        Nombre      = reader.GetString(1),
        Descripcion = reader.GetString(2),
        Precio      = reader.GetDecimal(3),
        Stock       = reader.GetInt32(4),
    };
}
