using Microsoft.Data.SqlClient;

namespace ProductosAPI.Data;

// Se encarga de crear las conexiones a SQL Server
public class Conexion
{
    private readonly string _cadenaConexion;

    public Conexion(IConfiguration configuration)
    {
        // Lee la cadena "ProductosDB" de appsettings.json
        _cadenaConexion = configuration.GetConnectionString("ProductosDB")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDB'");
    }

    // Devuelve una conexión nueva (se abre en el repositorio y se cierra con using)
    public SqlConnection ObtenerConexion() => new SqlConnection(_cadenaConexion);
}
