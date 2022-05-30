-- 1 Create Database
CREATE DATABASE Minions;          

-- 2 Create Tables
USE Minions                       
CREATE TABLE Minions
(
	Id INT,
	[Name] NVARCHAR(50),
	Age INT
);

USE Minions                       
CREATE TABLE Towns
(
	Id INT,
	[Name] NVARCHAR(100),
);

-- 3 Alter Minions Table
ALTER TABLE Minions 
ADD TownId INT
FOREIGN KEY (TownId) REFERENCES Towns(Id);

-- 4 Insert Records in Both Tables
INSERT INTO Towns (Id,Name)
VALUES 
(1,'Sofia'),
(2,'Plovdiv'),
(3,'Varna');

INSERT INTO Minions (Id,[Name],Age,TownId)
VALUES 
(1, 'Kevin', 22,1),
(2,'Bob',15,3),
(3,'Steward',NULL,2);

SELECT * FROM Minions
SELECT * FROM Towns

-- 5 Truncate Table Minions
TRUNCATE TABLE Minions;
SELECT * FROM Minions;

-- 6 Drop All Tables
DROP TABLE Minions;
DROP TABLE Towns;
--SELECT * FROM Minions; NOW TROW EXEPTION

