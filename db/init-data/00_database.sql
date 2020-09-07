CREATE DATABASE main
GO

USE [main]

CREATE TABLE dbo.Example(
  Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY CLUSTERED,
  ExampleInt int NOT NULL,
  ExampleBit bit NOT NULL
)
GO
