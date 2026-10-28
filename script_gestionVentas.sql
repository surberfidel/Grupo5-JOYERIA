USE [master]
GO


CREATE DATABASE [gestionVentas]
GO

USE [gestionVentas]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[Dni] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DetalleVentas](
	[IdDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdVenta] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Subtotal] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Productos](
	[IdProducto] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](150) NOT NULL,
	[Categoria] [varchar](50) NOT NULL,
	[Marca] [varchar](50) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[ImagenUrl] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Ventas](
	[IdVenta] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


SET IDENTITY_INSERT [dbo].[Clientes] ON 
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (1, N'Juan', N'Pérez', N'38123456')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (3, N'Lucas', N'Rodriguez', N'42111222')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (4, N'María', N'Gómez', N'27456789')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (5, N'Sofía', N'Martínez', N'35987654')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (6, N'Alejandro', N'Benítez', N'40333222')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (7, N'Valeria', N'López', N'33555777')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (8, N'Matías', N'Fernández', N'37888999')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido], [Dni]) VALUES (9, N'Camila', N'Romero', N'44123123')
SET IDENTITY_INSERT [dbo].[Clientes] OFF
GO

SET IDENTITY_INSERT [dbo].[Productos] ON 
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (1, N'Anillo Swarovski', N'Anillos', N'Swarovski', CAST(48000.00 AS Decimal(18, 2)), 3, N'producto_1.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (2, N'Anillo Pandora', N'Anillos', N'Pandora', CAST(42000.00 AS Decimal(18, 2)), 3, N'producto_2.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (4, N'Collar Swarovski', N'Collares', N'Swarovski', CAST(55000.00 AS Decimal(18, 2)), 3, N'producto_4.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (5, N'Collar Pandora', N'Collares', N'Pandora', CAST(50000.00 AS Decimal(18, 2)), 3, N'producto_5.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (6, N'Collar Gucci', N'Collares', N'Gucci', CAST(72000.00 AS Decimal(18, 2)), 3, N'producto_6.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (7, N'Pulsera Pandora', N'Pulseras', N'Pandora', CAST(46000.00 AS Decimal(18, 2)), 3, N'producto_7.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (8, N'Pulsera Tous', N'Pulseras', N'Tous', CAST(41000.00 AS Decimal(18, 2)), 3, N'producto_8.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (9, N'Pulsera Swarovski', N'Pulseras', N'Swarovski', CAST(45000.00 AS Decimal(18, 2)), 3, N'producto_9.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (10, N'Aros Swarovski', N* 'Aros', N'Swarovski', CAST(36000.00 AS Decimal(18, 2)), 3, N'producto_10.jpg')
INSERT [dbo].[Productos] ([IdProducto], [Nombre], [Categoria], [Marca], [PrecioUnitario], [Stock], [ImagenUrl]) VALUES (11, N'Anillo Tous', N'Anillos', N'Tous', CAST(39000.00 AS Decimal(18, 2)), 3, N'producto_11.jpg')
SET IDENTITY_INSERT [dbo].[Productos] OFF
GO


ALTER TABLE [dbo].[Ventas] ADD  DEFAULT (getdate()) FOR [Fecha]
GO

ALTER TABLE [dbo].[DetalleVentas]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Productos] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Productos] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleVentas] CHECK CONSTRAINT [FK_Detalle_Productos]
GO

ALTER TABLE [dbo].[DetalleVentas]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Ventas] FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Ventas] ([IdVenta])
GO
ALTER TABLE [dbo].[DetalleVentas] CHECK CONSTRAINT [FK_Detalle_Ventas]
GO

ALTER TABLE [dbo].[Ventas]  WITH CHECK ADD  CONSTRAINT [FK_Ventas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Ventas] CHECK CONSTRAINT [FK_Ventas_Clientes]
GO
