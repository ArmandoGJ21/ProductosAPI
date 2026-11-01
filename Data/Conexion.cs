// =====================================================================
//  Conexion.cs
//  PUNTO A RESOLVER: Conexión
//  Clase encargada de crear las conexiones a SQL Server (en Docker).
// =====================================================================

using Microsoft.Data.SqlClient;

namespace ProductosAPI.Data;

public class Conexion
{
    // Texto con los datos para conectarse: servidor, base, usuario y
    // contraseña. Se guarda como readonly porque no cambia nunca.
    private readonly string _cadenaConexion;

    // El constructor recibe IConfiguration, que .NET llena solo con lo
    // que hay en appsettings.json (inyección de dependencias).
    public Conexion(IConfiguration configuration)
    {
        // Busca "ConnectionStrings:ProductosDB" en appsettings.json.
        // Si no existe, lanza un error claro en lugar de fallar después.
        _cadenaConexion = configuration.GetConnectionString("ProductosDB")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDB'");
    }

    // Crea y devuelve una conexión NUEVA (todavía cerrada).
    // El repositorio la abre con Open() y la cierra sola al terminar
    // gracias a "using". Se crea una por operación para no dejar
    // conexiones abiertas.
    public SqlConnection ObtenerConexion() => new SqlConnection(_cadenaConexion);
}
