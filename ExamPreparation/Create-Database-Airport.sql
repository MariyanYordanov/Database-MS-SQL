--01. DDL
CREATE DATABASE Airport

USE Airport

CREATE TABLE Passengers (
	Id INT PRIMARY KEY IDENTITY,
	FullName VARCHAR(100) UNIQUE NOT NULL,
	Email VARCHAR(50) UNIQUE NOT NULL 
)

CREATE TABLE Pilots (
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) UNIQUE NOT NULL,
	LastName VARCHAR(30) UNIQUE NOT NULL,
	Age TINYINT CHECK (Age BETWEEN 21 AND 62) NOT NULL,
	Rating FLOAT CHECK (Rating BETWEEN 0.0 AND 10.0) 
)

CREATE TABLE AircraftTypes (
	Id INT PRIMARY KEY IDENTITY,
	TypeName VARCHAR(30) NOT NULL UNIQUE
)

CREATE TABLE Aircraft (
	Id INT PRIMARY KEY IDENTITY,
	Manufacturer VARCHAR(25) NOT NULL,
	Model VARCHAR(30) NOT NULL,
	[Year] INT NOT NULL,
	FlightHours INT,
	Condition CHAR(1) NOT NULL,
	TypeId INT FOREIGN KEY REFERENCES AircraftTypes(Id)  NOT NULL
)

CREATE TABLE PilotsAircraft (
	AircraftId INT FOREIGN KEY REFERENCES Aircraft(Id) NOT NULL,
	PilotId INT FOREIGN KEY REFERENCES Pilots(Id) NOT NULL,
	PRIMARY KEY (AircraftId, PilotId)
)

CREATE TABLE Airports (
	Id INT PRIMARY KEY IDENTITY,
	AirportName VARCHAR(70) UNIQUE NOT NULL,
	Country VARCHAR(100) UNIQUE NOT NULL
)

CREATE TABLE FlightDestinations (
	Id INT PRIMARY KEY IDENTITY,
	AirportId INT FOREIGN KEY REFERENCES Airports(Id) NOT NULL,
	[Start] DATETIME NOT NULL,
	AircraftId INT FOREIGN KEY REFERENCES Aircraft(Id) NOT NULL,
	PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL,
	TicketPrice DECIMAL(16,2) DEFAULT 15.00 NOT NULL
)

GO

-- Fullfill database

-- 02. Insert
INSERT INTO Passengers (FullName, Email)
SELECT CONCAT(p.FirstName, ' ', p.LastName)
    AS FullName,
	   CONCAT(p.FirstName,p.LastName,'@gmail.com')
    AS Email
  FROM Pilots AS p
 WHERE p.Id >= 5 AND p.Id <=15

GO

-- 03. Update

UPDATE Aircraft
   SET Condition = 'A' 
 WHERE (Condition = 'C' OR Condition = 'B')
   AND (FlightHours IS NULL OR FlightHours <=100)
   AND ([Year] >= 2013)

GO

-- 04. Delete
DELETE
  FROM Passengers 
 WHERE LEN(FullName) <= 10

-- 05. Aircraft
  SELECT Manufacturer,
         Model,
         FlightHours,
         Condition
    FROM Aircraft 
ORDER BY FlightHours
	DESC

GO

-- 06. Pilots and Aircraft
  SELECT FirstName,
         LastName,
         Manufacturer,
         Model,
         FlightHours
    FROM Pilots 
      AS p
    JOIN PilotsAircraft 
      AS pa
  	  ON pa.PilotId = p.Id
    JOIN Aircraft 
      AS a
      ON a.Id = pa.AircraftId
   WHERE FlightHours IS NOT NULL
     AND FlightHours <= 304
ORDER BY FlightHours
   DESC, FirstName
		 
GO

-- 07.Top 20 Flight Destinations
SELECT TOP(20) fd.Id
			AS DestinationId,
			   [Start],
			   pass.FullName,
			   a.AirportName,
			   fd.TicketPrice
          FROM FlightDestinations AS fd
     LEFT JOIN Passengers AS pass ON pass.Id = fd.PassengerId
     LEFT JOIN Airports AS a ON a.Id = fd.AirportId
         WHERE DATEPART(DAY,[Start]) % 2  = 0 
	  ORDER BY fd.TicketPrice
		 DESC, a.AirportName

GO

-- 08.Number of Flights for Each Aircraft
   SELECT a.Id AS AircraftId,
	      a.Manufacturer,
	      a.FlightHours,
	      COUNT(fd.AircraftId) AS FlightDestinationsCount,
	      ROUND(AVG(fd.TicketPrice),2) AS AvgPrice
     FROM FlightDestinations AS fd 
LEFT JOIN Aircraft AS a  ON fd.AircraftId = a.Id
 GROUP BY a.Id,a.Manufacturer,a.FlightHours
   HAVING COUNT(fd.AircraftId) >= 2
 ORDER BY FlightDestinationsCount DESC,a.Id

 GO 

 -- 09.Regular Passengers
  SELECT p.FullName,
 		 COUNT(fd.AircraftId) AS CountOfAircraft,
 		 SUM(fd.TicketPrice) AS TotalPayed
    FROM Passengers AS p
    JOIN FlightDestinations AS fd ON fd.PassengerId = p.Id
   WHERE p.FullName LIKE '_a%'
GROUP BY p.FullName
  HAVING COUNT(fd.AircraftId) > 1
ORDER BY p.FullName 

GO 

-- 10. Full Info for Flight Destinations
   SELECT a.AirportName,
	      fd.[Start] AS DayTime,
	      fd.TicketPrice,
	      p.FullName,
	      ac.Manufacturer,
	      ac.Model
     FROM FlightDestinations AS fd
LEFT JOIN Airports AS a ON a.Id = fd.AirportId
LEFT JOIN Passengers AS p ON p.Id = fd.PassengerId
LEFT JOIN Aircraft AS ac ON ac.Id = fd.AircraftId
    WHERE DATEPART(HOUR,fd.Start) >= 6 
      AND DATEPART(HOUR,fd.Start) <= 20
      AND fd.TicketPrice > 2500
 ORDER BY ac.Model

 GO

 -- 11. Find all Destinations by Email Address
 CREATE FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(50))
 RETURNS INT
 AS
 BEGIN
		DECLARE @result INT = 
		(SELECT COUNT(fd.Id) 
		FROM FlightDestinations AS fd
		LEFT JOIN Passengers AS p ON p.Id = fd.PassengerId
		WHERE p.Email = @email)
		RETURN @result
 END

 GO

 SELECT dbo.udf_FlightDestinationsByEmail ('PierretteDunmuir@gmail.com')

 GO

 -- 12. Full Info for Airports
 CREATE OR ALTER PROC usp_SearchByAirportName (@airportName VARCHAR(70))
 AS
 BEGIN 
		    

		 SELECT @airportName AS AirportName,
			    p.FullName,
			     CASE
		         WHEN fd.TicketPrice <= 400 THEN 'Low'
		         WHEN fd.TicketPrice BETWEEN 401 AND 1500  THEN 'Medium'
		         WHEN fd.TicketPrice > 1501 THEN 'High'
			   END AS LevelOfTickerPrice,
			    ac.Manufacturer,
			    ac.Condition,
			    acty.TypeName
		 FROM Airports AS a 
    LEFT JOIN FlightDestinations AS fd ON fd.AirportId = a.Id 
    LEFT JOIN Passengers AS p ON p.Id = fd.PassengerId
    LEFT JOIN Aircraft AS ac ON ac.Id = fd.AircraftId
    LEFT JOIN AircraftTypes AS acty ON acty.Id = ac.TypeId
 END

 GO

 EXEC usp_SearchByAirportName 'Sir Seretse Khama International Airport'

 GO