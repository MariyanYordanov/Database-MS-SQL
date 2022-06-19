CREATE DATABASE Zoo

USE Zoo

GO

--1.Database design
CREATE TABLE Owners (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	[Address] VARCHAR(50)
)

CREATE TABLE AnimalTypes (
	Id INT PRIMARY KEY IDENTITY,
	AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages (
	Id INT PRIMARY KEY IDENTITY,
	AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes(Id) NOT NULL
)

CREATE TABLE Animals (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL,
	BirthDate DATE NOT NULL,
	OwnerId INT FOREIGN KEY REFERENCES Owners(Id),
	AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes(Id) NOT NULL
)

CREATE TABLE AnimalsCages (
	CageId INT FOREIGN KEY REFERENCES Cages(Id) NOT NULL,
	AnimalId INT FOREIGN KEY REFERENCES Animals(Id) NOT NULL,
	PRIMARY KEY (CageId,AnimalId)
)

CREATE TABLE VolunteersDepartments (
	Id INT PRIMARY KEY IDENTITY,
	DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	[Address] VARCHAR(50),
	AnimalId INT FOREIGN KEY REFERENCES Animals(Id),
	DepartmentId INT FOREIGN KEY REFERENCES VolunteersDepartments(Id) NOT NULL
)

-- 2.Insert
INSERT INTO Volunteers ([Name],PhoneNumber,[Address],AnimalId,DepartmentId)
VALUES
('Anita Kostova','0896365412','Sofia, 5 Rosa str.',15,1),
('Dimitur Stoev','0877564223',NULL,42,4),
('Kalina Evtimova','0896321112','Silistra, 21 Breza str.',9,7),
('Stoyan Tomov','0898564100','Montana, 1 Bor str.',18,8),
('Boryana Mileva','0888112233',NULL,31,5)

INSERT INTO Animals ([Name],BirthDate,OwnerId,AnimalTypeId)
VALUES
('Giraffe','2018-09-21',21,1),
('Harpy Eagle','2015-04-17',15,3),
('Hamadryas Baboon','2017-11-02',NULL,1),
('Tuatara','2021-06-30',2,4)

-- 3.Update
UPDATE Animals
SET OwnerId = 14
WHERE Animals.OwnerId IS NULL

-- 4.Delete
DELETE FROM Volunteers WHERE DepartmentId = 2

DELETE FROM VolunteersDepartments WHERE DepartmentName = 'Education program assistant'

-- 5.Volunteers
SELECT [Name],PhoneNumber,[Address],AnimalId,DepartmentId
FROM Volunteers ORDER BY [Name],AnimalId,DepartmentId

-- 6.Animals data
SELECT a.[Name], anty.AnimalType, FORMAT(a.BirthDate,'dd.MM.yyyy')
FROM Animals AS a
LEFT JOIN AnimalTypes AS anty ON a.AnimalTypeId = anty.Id
ORDER BY a.[Name]

-- 7.Owners and Their Animals
SELECT TOP(5) o.[Name] AS Owner,COUNT(a.OwnerId) AS CountOfAnimals
FROM Owners AS o JOIN Animals AS a ON o.Id = a.OwnerId
GROUP BY o.[Name] ORDER BY CountOfAnimals DESC,o.[Name]

-- 8.Owners, Animals and Cages
SELECT CONCAT(o.[Name],'-' ,a.[Name]) AS OwnersAnimals, o.PhoneNumber ,c.Id AS CageId
FROM Owners AS o 
JOIN Animals AS a ON o.Id = a.OwnerId
JOIN AnimalsCages AS ac ON ac.AnimalId = a.Id
JOIN Cages AS c ON ac.CageId = c.AnimalTypeId
JOIN AnimalTypes AS aty ON aty.AnimalType = a.Id
WHERE aty.AnimalType = 'Mammals'
ORDER BY o.[Name],a.[Name] DESC


-- 9.Volunteers in Sofia
SELECT v.[Name], v.PhoneNumber, 
		SUBSTRING(v.[Address],CHARINDEX(',',v.[Address]) + 2,LEN(v.[Address]))
FROM Volunteers AS v
WHERE v.DepartmentId = 2 AND v.[Address] LIKE '%Sofia%' 
ORDER BY v.[Name]

-- 10.Animals for Adoption
SELECT a.[Name], YEAR(a.BirthDate) AS BirthYear, anty.AnimalType
FROM Animals AS a
LEFT JOIN AnimalTypes AS anty ON a.AnimalTypeId = anty.Id
WHERE OwnerId IS NULL 
AND a.BirthDate > '2018-01-01'
AND a.AnimalTypeId != 3
ORDER BY a.[Name]

GO
-- 11.All Volunteers in a Department
CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(30))
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(*) FROM VolunteersDepartments AS vd
				  LEFT JOIN Volunteers AS v ON v.DepartmentId = vd.Id
				  WHERE vd.DepartmentName = @VolunteersDepartment)
END

GO

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Education program assistant')
SELECT dbo.udf_GetVolunteersCountFromADepartment ('Guest engagement')
SELECT dbo.udf_GetVolunteersCountFromADepartment ('Zoo events')

GO

-- 12.Animals with Owner or Not
CREATE OR ALTER PROCEDURE usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30))
AS
BEGIN

SELECT a.[Name], 
	   o.PhoneNumber,
	   o.[Name] AS OwnersName
FROM Owners AS o 
LEFT JOIN Animals AS a ON a.OwnerId = o.Id 
WHERE a.[Name] = @AnimalName

END

GO

EXEC usp_AnimalsWithOwnersOrNot 'Pumpkinseed Sunfish'
EXEC usp_AnimalsWithOwnersOrNot 'Hippo'