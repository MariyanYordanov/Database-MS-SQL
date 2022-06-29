-- 1.Create databade CigarShop
CREATE DATABASE CigarShop

USE CigarShop

GO

CREATE TABLE Sizes (
	Id INT PRIMARY KEY IDENTITY,
	[Length] INT CHECK([Length] BETWEEN 10 AND 25) NOT NULL,
	RingRange DECIMAL(16,2) CHECK(RingRange BETWEEN 1.5 AND 7.5) NOT NULL
)

CREATE TABLE Tastes (
	Id INT PRIMARY KEY IDENTITY,
	TasteType VARCHAR(20) NOT NULL,
	TasteStrength VARCHAR(15) NOT NULL,
	ImageURL VARCHAR(100) NOT NULL
)

CREATE TABLE Brands (
	Id INT PRIMARY KEY IDENTITY,
	BrandName VARCHAR(20) NOT NULL UNIQUE,
	BrandDescription VARCHAR(MAX) 
)

CREATE TABLE Cigars (
	Id INT PRIMARY KEY IDENTITY,
	CigarName VARCHAR(80) NOT NULL,
	BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
	TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
	SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
	PriceForSingleCigar MONEY NOT NULL,
	ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses (
	Id INT PRIMARY KEY IDENTITY,
	Town VARCHAR(30) NOT NULL,
	Country NVARCHAR(30) NOT NULL,
	Streat NVARCHAR(100) NOT NULL,
	ZIP VARCHAR(20) NOT NULL
)

CREATE TABLE Clients (
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE ClientsCigars (
	ClientId INT FOREIGN KEY REFERENCES Clients(Id),
	CigarId INT FOREIGN KEY REFERENCES Cigars(Id),
	PRIMARY KEY (ClientId,CigarId)
)

GO

-- DDL

-- 2.Insert
INSERT INTO Cigars 
(CigarName, BrandId, TastId, SizeId, PriceForSingleCigar ,ImageURL)
VALUES 
('COHIBA ROBUSTO', 9,1, 5, 15.50, 'cohiba-robusto-stick_18.jpg'),
('COHIBA SIGLO I', 9, 1, 10, 410.00, 'cohiba-siglo-i-stick_12.jpg'),
('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5,11, 7.50, 'hoyo-du-maire-stick_17.jpg'),
('HOYO DE MONTERREY LE HOYO DE SAN JUAN', 14, 4, 15, 32.00, 'hoyo-de-san-juan-stick_20.jpg'),
('TRINIDAD COLONIALES', 2, 3, 8, 85.21, 'trinidad-coloniales-stick_30.jpg')

INSERT INTO Addresses (Town, Country, Streat, ZIP) 
VALUES
('Sofia', 'Bulgaria', '18 Bul. Vasil levski', '1000'),
('Athens', 'Greece', '4342 McDonald Avenue', '10435'),
('Zagreb', 'Croatia', '4333 Lauren Drive', '10000')

-- 3.Update
UPDATE Cigars 
SET PriceForSingleCigar *= 1.20
WHERE TastId = 1

UPDATE Brands
SET BrandDescription = 'New description'
WHERE BrandDescription IS NULL

-- 4.Delete
DELETE FROM Clients
WHERE AddressId IN
	(SELECT Id
	FROM Addresses
	WHERE Country LIKE 'C%')

DELETE 
FROM Addresses
WHERE LEFT(Country, 1) = 'C'

GO

-- 5.Cigars by Price

SELECT CigarName, PriceForSingleCigar, ImageURL
FROM Cigars
ORDER BY PriceForSingleCigar, CigarName DESC

GO

-- 6.Cigars by Taste
SELECT c.Id, 
	   c.CigarName, 
	   c.PriceForSingleCigar, 
	   t.TasteType, 
	   t.TasteStrength
FROM Cigars AS c 
INNER JOIN Tastes AS t ON c.TastId = t.Id
WHERE (t.TasteType = 'Earthy') OR (t.TasteType = 'Woody')
ORDER BY c.PriceForSingleCigar DESC

GO

-- 7.Clients without Cigars
SELECT c.Id,
	   CONCAT(FirstName,' ',LastName) AS ClientName, 
	   c.Email
FROM Clients AS c
WHERE NOT EXISTS
(SELECT 1 FROM ClientsCigars AS cc WHERE cc.ClientId = c.Id)
-- RETURN ONE IF HAVE MATCH -> cc.ClientId = c.Id
ORDER BY ClientName ASC

GO

-- 8.First 5 Cigars
SELECT TOP(5) c.CigarName, c.PriceForSingleCigar, c.ImageURL
FROM Cigars AS c 
LEFT JOIN Sizes AS s ON c.SizeId = s.Id
WHERE s.[Length] >= 12 
AND (c.CigarName LIKE '%ci%' OR c.PriceForSingleCigar > 50)
AND s.RingRange > 2.55
ORDER BY c.CigarName ASC, c.PriceForSingleCigar DESC

GO

-- 9.Clients with ZIP Codes
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS FullName,
	   a.Country,
	   a.ZIP,
	   CONCAT('$',
	   (SELECT MAX(PriceForSingleCigar)
	     FROM Cigars AS cg 
	     JOIN ClientsCigars AS cc 
	       ON cg.Id = cc.CigarId AND cc.ClientId = c.Id ))  AS CigarPrice
FROM Clients AS c
JOIN Addresses AS a ON a.Id = c.AddressId
WHERE ISNUMERIC(a.ZIP) = 1
ORDER BY FullName ASC

GO

-- 10.Cigars by Size
SELECT c.LastName, 
	   AVG(s.[Length]) AS CiagrLength,
	   CEILING(AVG(s.RingRange))AS CiagrRingRange
FROM Clients AS c
JOIN ClientsCigars AS cc ON cc.ClientId = c.Id
JOIN Cigars AS cg ON cg.Id = cc.CigarId
JOIN Sizes AS s ON s.Id = cg.SizeId
GROUP BY c.LastName
ORDER BY AVG(s.[Length]) DESC

GO

-- 11.Client with Cigars
CREATE FUNCTION udf_ClientWithCigars(@name VARCHAR(30))
RETURNS INT
AS 
BEGIN
	DECLARE @result int = (SELECT COUNT(cc.CigarId) 
							FROM ClientsCigars AS cc
							JOIN Clients as c on c.Id = cc.ClientId
							WHERE c.FirstName = @name )
	 RETURN @result 
END

GO 

SELECT dbo.udf_ClientWithCigars('Betty')

GO
-- 12.Search for Cigar with Specific Taste
CREATE PROCEDURE usp_SearchByTaste(@taste VARCHAR(20))
AS
BEGIN
	SELECT cg.CigarName, 
		   CONCAT('$',cg.PriceForSingleCigar) AS Price, 
		   t.TasteType, 
		   b.BrandName, 
		   CONCAT(s.[Length],' ','cm') AS CigarLength, 
		   CONCAT(s.RingRange,' ','cm') AS CigarRingRange
	  FROM Cigars AS cg
	  JOIN Sizes AS s ON s.Id = cg.SizeId
	  JOIN Tastes AS t ON t.Id = cg.TastId
	  JOIN Brands AS b ON b.Id = cg.BrandId
	  WHERE t.TasteType = @taste
	  ORDER BY CigarLength ASC, CigarRingRange DESC
END

GO

EXEC usp_SearchByTaste 'Woody'
