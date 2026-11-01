-- =====================================================================
--  script.sql
--  Crea la base de datos ProductosDB, la tabla Productos y unos datos
--  de ejemplo. Docker lo ejecuta automáticamente (servicio "db-init").
--
--  Se puede ejecutar varias veces sin error: antes de crear cada cosa
--  revisa si ya existe (IF ... IS NULL).
-- =====================================================================

-- 1) Crear la base de datos solo si todavía no existe
IF DB_ID('ProductosDB') IS NULL
    CREATE DATABASE ProductosDB;
GO

-- 2) Cambiar a la base recién creada para que lo siguiente se haga ahí
USE ProductosDB;
GO

-- 3) Crear la tabla Productos solo si todavía no existe
IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        -- Llave primaria. IDENTITY(1,1) = SQL Server genera el Id solo,
        -- empezando en 1 y sumando 1 por cada registro nuevo
        Id          INT IDENTITY(1,1) PRIMARY KEY,

        -- Nombre del producto (obligatorio, máximo 100 caracteres)
        Nombre      NVARCHAR(100)  NOT NULL,

        -- Descripción opcional; si no se manda queda como texto vacío
        Descripcion NVARCHAR(250)  NOT NULL DEFAULT '',

        -- Precio con 2 decimales (hasta 99,999,999.99)
        Precio      DECIMAL(10,2)  NOT NULL,

        -- Cantidad disponible en inventario
        Stock       INT            NOT NULL DEFAULT 0
    );

    -- 4) Datos de ejemplo para probar desde Swagger
    INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock) VALUES
        (N'Teclado',  N'Teclado mecánico USB',  850.00, 10),
        (N'Mouse',    N'Mouse inalámbrico',     350.50, 25),
        (N'Monitor',  N'Monitor 24 pulgadas',  3200.00,  5);
END
GO
