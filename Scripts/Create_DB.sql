CREATE DATABASE [BD_Shop]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BD_Shop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BD_Shop.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'@BD_Shop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BD_Shop.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT

GO

USE [BD_Shop]

CREATE TABLE [dbo].[Categories](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_Сategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Products](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](50) NULL,
	[Price] [DECIMAL](18, 0) NULL,
	[Category_id] [INT] NULL,
 CONSTRAINT [PK_Products12] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Сategories] FOREIGN KEY([Category_id])
REFERENCES [dbo].[Categories] ([Id])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Сategories]
GO

INSERT INTO [dbo].[Categories]
           ([Name])
     VALUES
           ('Продукты'),
		   ('Хоз товары'),
		   ('Товары для животных')
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Price]
           ,[Category_id])
     VALUES
           ('Мыло', 150.0, 2),
		   ('Макароны', 50.0, 1),
		   ('Молоко', 70.0, 1)
GO