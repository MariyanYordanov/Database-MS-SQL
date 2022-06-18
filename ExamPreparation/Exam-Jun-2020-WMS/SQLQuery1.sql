CREATE DATABASE WMS

USE WMS

GO

-- 1.Database design
CREATE TABLE Clients (
	ClientId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone CHAR(12) CHECK (LEN(Phone)  = 12) NOT NULL
)


CREATE TABLE Mechanics (
	MechanicId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	[Address]  VARCHAR(255) NOT NULL
)


CREATE TABLE Models (
	ModelId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) UNIQUE NOT NULL
)


CREATE TABLE Jobs (
	JobId INT PRIMARY KEY IDENTITY,
	ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
	[Status] VARCHAR(11) DEFAULT 'Pending' CHECK ([Status] IN ('Pending','In Progress','Finished')) NOT NULL,
	ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL,
	MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE
)


CREATE TABLE Orders (
	OrderId INT PRIMARY KEY IDENTITY,
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	IssueDate DATE,
	Delivered BIT DEFAULT 0 NOT NULL
)


CREATE TABLE Vendors (
	VendorId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) UNIQUE NOT NULL
)


CREATE TABLE Parts (
	PartId INT PRIMARY KEY IDENTITY,
	SerialNumber VARCHAR(50) UNIQUE NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	Price MONEY CHECK (Price < 10000) NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL,
	StockQty INT DEFAULT 0 NOT NULL
)


CREATE TABLE OrderParts (
	OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	PRIMARY KEY (OrderId,PartId),
	Quantity INT DEFAULT 1 CHECK (Quantity > 0) NOT NULL
)


CREATE TABLE PartsNeeded (
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	PRIMARY KEY (JobId,PartId),
	Quantity INT DEFAULT 1 CHECK (Quantity > 0) NOT NULL
)

-- 2.Insert 
INSERT INTO Clients
(FirstName,LastName,Phone)
VALUES
('Teri','Ennaco','570-889-5187'),
('Merlyn','Lawler','201-588-7810'),
('Georgene','Montezuma','925-615-5185'),
('Jettie','Mconnell','908-802-3564'),
('Lemuel','Latzke','631-748-6479'),
('Melodie','Knipp','805-690-1682'),
('Candida','Corbley','908-275-8357')

INSERT INTO Parts
(SerialNumber,[Description],Price,VendorId)
VALUES
('WP8182119','Door Boot Seal',117.86,2),
('W10780048','Suspension Rod',42.81,1),
('W10841140','Silicone Adhesive ',6.77,4),
('WPY055980','High Temperature Adhesive',13.94,3)

-- 3.Update
UPDATE Jobs
SET MechanicId = 3, [Status] = 'In Progress'
WHERE [Status] = 'Pending'

-- 4.Delete
DELETE FROM OrderParts
WHERE OrderId = 19

DELETE FROM Orders
WHERE OrderId = 19

-- 5.Mechanic Assignments
SELECT CONCAT(m.FirstName,' ',m.LastName), j.[Status], j.IssueDate
FROM Mechanics AS m
JOIN Jobs AS j ON j.MechanicId = m.MechanicId
ORDER BY m.MechanicId,j.IssueDate,j.JobId

-- 6.Current Clients
SELECT CONCAT(c.FirstName,' ',c.LastName) AS Client,
	   DATEDIFF(DAY,j.IssueDate,'2017-04-24') AS [Days going],
	   j.[Status]
FROM Clients AS c 
JOIN Jobs AS j ON c.ClientId = j.ClientId
WHERE j.[Status] = 'In Progress'
ORDER BY [Days going] DESC, c.ClientId

-- 7.Mechanic Performance
 SELECT r.Mechanic, AVG(r.[Days]) AS [Average Days]
   FROM (
 		SELECT CONCAT(m.FirstName,' ',m.LastName) AS Mechanic,
 		DATEDIFF(DAY,j.IssueDate,j.FinishDate) AS [Days],
		m.MechanicId AS ID
 		FROM Mechanics AS m
 		JOIN Jobs AS j ON j.MechanicId = m.MechanicId
 		WHERE j.[Status] = 'Finished'
 		) AS r
GROUP BY r.Mechanic,r.ID
ORDER BY r.ID

-- 8.Available Mechanics
SELECT CONCAT(FirstName,' ',LastName) AS Available
FROM Mechanics m
LEFT JOIN Jobs j ON j.MechanicId = m.MechanicId
WHERE 'Finished' = 
ALL (SELECT j.[Status]
     FROM Jobs j 
	 WHERE j.MechanicId = m.MechanicId)
GROUP BY FirstName, LastName, m.MechanicId
ORDER BY m.MechanicId

SELECT CONCAT(m.FirstName,' ',m.LastName) AS Available
FROM Mechanics AS m
LEFT JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE (SELECT COUNT(j.JobId)
       FROM Jobs AS j
       WHERE j.[Status] <> 'Finished' 
	   AND j.MechanicId = m.MechanicId
	   GROUP BY j.MechanicId, j.[Status]) IS NULL
GROUP BY m.MechanicId,m.FirstName,m.LastName


--9.Past Expenses
SELECT j.JobId, ISNULL(SUM(p.Price * op.Quantity),0) AS Total
FROM Jobs AS j
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON op.OrderId = o.OrderId
LEFT JOIN Parts AS p ON p.PartId = op.PartId
WHERE j.Status = 'Finished'
GROUP BY j.JobId
ORDER BY Total DESC,j.JobId

-- 10.Missing Parts
SELECT p.PartId, 
	   p.[Description],
	   pn.Quantity AS [Required],
	   p.StockQty AS InStock,
	   IIF(o.Delivered = 0, op.Quantity,0) AS Ordered
FROM Parts AS p
LEFT JOIN PartsNeeded AS pn ON pn.PartId = p.PartId
LEFT JOIN Jobs AS j ON j.JobId = pn.JobId
LEFT JOIN OrderParts AS op ON op.PartId = p.PartId
LEFT JOIN Orders AS o ON o.JobId = j.JobId 
WHERE j.Status <> 'Finished'
AND pn.Quantity > p.StockQty + IIF(o.Delivered = 0, op.Quantity,0)
ORDER BY  p.PartId

GO

-- 11.Place Order
CREATE PROCEDURE usp_PlaceOrder (@jobId INT, @serialNumber VARCHAR(50), @quantity INT)
AS

DECLARE @status VARCHAR(10) = (SELECT [Status] from Jobs WHERE JobId = @jobId)

DECLARE @partId VARCHAR(10) = (SELECT PartId FROM Parts WHERE SerialNumber = @serialNumber)

	IF (@quantity <= 0)
		THROW 50012, 'Part quantity must be more than zero!', 1
	ELSE IF (@status IS NULL)
		THROW 50013, 'Job not found!', 1
	ELSE IF (@status = 'Finished')
		THROW 50011, 'This job is not active!', 1
	ELSE IF (@serialNumber IS NULL)
		THROW 50014 , 'Part not found!', 1

DECLARE @orderId INT = (SELECT o.OrderId FROM Orders AS o
						WHERE JobId = @jobId AND o.IssueDate IS NULL)

	IF (@orderId IS NULL)
		BEGIN
			INSERT INTO Orders (JobId,IssueDate)
			VALUES (@jobId, NULL)
		END

SET @orderId = (SELECT OrderId FROM Orders AS o 
				WHERE o.JobId = @jobId AND o.IssueDate IS NULL)

DECLARE @orderPartExists INT =(SELECT OrderId FROM OrderParts 
							   WHERE OrderId = @orderId AND PartId = @partId)
											
	IF (@orderPartExists IS NULL)
		BEGIN
			INSERT INTO OrderParts (OrderId,PartId,Quantity)
			VALUES (@orderId,@partId,@quantity)
		END
	ELSE
		BEGIN
			UPDATE OrderParts 
			SET Quantity += @quantity
			WHERE OrderId = @orderId AND PartId = @partId
		END

GO 

DECLARE @err_msg AS NVARCHAR(MAX);
BEGIN TRY
  EXEC usp_PlaceOrder 1, 'ZeroQuantity', 0
END TRY

BEGIN CATCH
  SET @err_msg = ERROR_MESSAGE();
  SELECT @err_msg
END CATCH

GO

-- 12.Cost Of Order
CREATE OR ALTER FUNCTION udf_GetCost(@jobId INT)
RETURNS DECIMAL (16,2)
AS
BEGIN
	RETURN  ISNULL((SELECT SUM(p.Price) AS Result
			FROM Jobs AS j 
			JOIN Orders AS o ON o.JobId = j.JobId
			JOIN OrderParts AS op ON op.OrderId = o.OrderId
			JOIN Parts AS p ON p.PartId = op.PartId
			WHERE o.JobId = @jobId
			GROUP BY j.JobId), 0)
END

GO

SELECT dbo.udf_GetCost(1)

GO