CREATE DATABASE [BD_Shop]
GO

USE [BD_Shop]

CREATE TABLE [dbo].[Categories]
(
	[Id] [INT] PRIMARY KEY IDENTITY(1,1),
	[Name] [NVARCHAR](50)
)
 

CREATE TABLE [dbo].[Products]
(
	[Id] [INT] PRIMARY KEY IDENTITY(1,1),
	[Name] [NVARCHAR](50),
	[Price] [DECIMAL](18, 0),
	[CategoryId] [INT],
	FOREIGN KEY([CategoryId])  REFERENCES [Categories](Id),
)
GO

INSERT INTO [dbo].[Categories]
           ([Name])
     VALUES
           (N'��������'),
		   (N'��� ������'),
		   (N'������ ��� ��������')
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Price]
           ,[CategoryId])
     VALUES
           (N'����', 150.0, 2),
		   (N'��������', 50.0, 1),
		   (N'������', 70.0, 1)
GO