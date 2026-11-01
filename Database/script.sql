-- Script para crear la base de datos de ProductosAPI (SQL Server)

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

    -- Datos de ejemplo
    INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock) VALUES
        ('Teclado',  'Teclado mecánico USB',    850.00, 10),
        ('Mouse',    'Mouse inalámbrico',        350.50, 25),
        ('Monitor',  'Monitor 24 pulgadas',     3200.00,  5);
END
GO
