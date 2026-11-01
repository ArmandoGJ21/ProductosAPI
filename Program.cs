// =====================================================================
//  Program.cs
//  Punto de entrada de la API. Aquí solo se configura todo y se
//  registran los endpoints; la lógica vive en las otras carpetas.
// =====================================================================

using ProductosAPI.Data;
using ProductosAPI.Endpoints;
using ProductosAPI.Repositories;

// 1) Crear el "constructor" de la aplicación.
//    Lee automáticamente appsettings.json (ahí está la cadena de conexión).
var builder = WebApplication.CreateBuilder(args);

// 2) Registrar Swagger para poder ver y probar los endpoints en el navegador
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) Inyección de dependencias:
//    Le decimos a .NET qué objetos debe crear cuando alguien los pida.
//
//    - Conexion como Singleton: se crea UNA sola vez para toda la app,
//      porque solo guarda la cadena de conexión.
builder.Services.AddSingleton<Conexion>();
//
//    - IProductoRepository como Scoped: se crea uno nuevo POR CADA
//      petición HTTP. Cuando un endpoint pida un IProductoRepository,
//      .NET le entregará un ProductoRepository.
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// 4) Construir la aplicación con todo lo registrado arriba
var app = builder.Build();

// 5) Activar Swagger solo en desarrollo (http://localhost:5210/swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6) Si alguien entra por http, lo redirige a https
app.UseHttpsRedirection();

// 7) Registrar las rutas /productos (están en Endpoints/ProductosEndpoints.cs)
ProductosEndpoints.Map(app);

// 8) Arrancar el servidor y quedarse escuchando peticiones.
//    Debe ir SIEMPRE al final.
app.Run();
