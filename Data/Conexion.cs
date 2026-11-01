using Microsoft.Data.SqlClient;

namespace ProductosAPI.Data;

public class Conexion
{
    private readonly string _cadenaConexion;
    public Conexion(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("ProductosDB")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDB'");
    }

    public SqlConnection ObtenerConexion() => new SqlConnection(_cadenaConexion);
}
