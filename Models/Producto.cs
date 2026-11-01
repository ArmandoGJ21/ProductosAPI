// =====================================================================
//  Producto.cs
//  Modelo (entidad). Cada propiedad corresponde a una columna de la
//  tabla Productos en SQL Server. También es la forma del JSON que
//  se recibe y se devuelve en Swagger.
// =====================================================================

namespace ProductosAPI.Models;

public class Producto
{
    public int     Id          { get; set; }

    public string  Nombre      { get; set; } = string.Empty;

    public string  Descripcion { get; set; } = string.Empty;

    public decimal Precio      { get; set; }

    public int     Stock       { get; set; }
}
