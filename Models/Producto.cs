// =====================================================================
//  Producto.cs
//  Modelo (entidad). Cada propiedad corresponde a una columna de la
//  tabla Productos en SQL Server. También es la forma del JSON que
//  se recibe y se devuelve en Swagger.
// =====================================================================

namespace ProductosAPI.Models;

public class Producto
{
    // Llave primaria. La genera SQL Server (IDENTITY), por eso no se
    // manda al crear un producto.
    public int     Id          { get; set; }

    // Nombre del producto. Se inicializa en "" para que nunca sea null.
    public string  Nombre      { get; set; } = string.Empty;

    // Descripción corta del producto (opcional).
    public string  Descripcion { get; set; } = string.Empty;

    // Precio. Se usa decimal (y no double) porque es dinero y no debe
    // perder centavos por redondeo.
    public decimal Precio      { get; set; }

    // Cantidad disponible en inventario.
    public int     Stock       { get; set; }
}
