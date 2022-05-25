CREATE DATABASE [OnlineStore];

USE [OnlineStore];
CREATE TABLE [Cities]
(
	[CityID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE [Customers]
(
	[CustomerID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[Birthday] DATE NOT NULL,
	[CityID] INT
	CONSTRAINT FK_Customres_Cities
	FOREIGN KEY ([CityID])
	REFERENCES [Cities]([CityID])
);

CREATE TABLE [Orders]
(
	[OrderID] INT PRIMARY KEY IDENTITY,
	[CustomerID] INT 
	CONSTRAINT FK_Orders_Customres
	FOREIGN KEY ([CustomerID])
	REFERENCES [Customers]([CustomerID])
);

CREATE TABLE [ItemTypes]
(
	[ItemTypeID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE [Items]
(
	[ItemID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[ItemTypeID] INT 
	CONSTRAINT FK_Items_ItemTypes
	FOREIGN KEY ([ItemTypeID])
	REFERENCES [ItemTypes]([ItemTypeID])
);

CREATE TABLE [OrderItems]
(
	[OrderID] INT,
	[ItemID] INT,

	CONSTRAINT PK_OrderItems
	PRIMARY KEY ([OrderID],[ItemID]),

	CONSTRAINT FK_OrderItems_Orders
	FOREIGN KEY ([OrderID])
	REFERENCES [Orders]([OrderID]),

	CONSTRAINT FK_OrderItems_Items
	FOREIGN KEY ([ItemID])
	REFERENCES [Items]([ItemID])
);