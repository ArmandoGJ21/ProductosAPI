IF DB_ID('ProductosDB') IS NULL
    CREATE DATABASE ProductosDB;
GO


USE ProductosDB;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100)  NOT NULL,
        Descripcion NVARCHAR(250)  NOT NULL DEFAULT '',
        Precio      DECIMAL(10,2)  NOT NULL,
        Stock       INT            NOT NULL DEFAULT 0
    );

    INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock) VALUES
        (N'Teclado',  N'Teclado mecánico USB',  850.00, 10),
        (N'Mouse',    N'Mouse inalámbrico',     350.50, 25),
        (N'Monitor',  N'Monitor 24 pulgadas',  3200.00,  5);
END
GO
