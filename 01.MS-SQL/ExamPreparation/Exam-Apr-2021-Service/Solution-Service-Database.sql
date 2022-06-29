CREATE DATABASE [Service]

USE [Service]

GO

-- DDL
-- 1.Table design
CREATE TABLE Users (
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	[Name] VARCHAR(50),
	Birthdate DATETIME,
	Age INT CHECK (Age between 14 and 110),
	Email VARCHAR(50) NOT NULL,
)

CREATE TABLE Departments (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName  VARCHAR(25),
	LastName VARCHAR(25),
	Birthdate DATETIME,
	Age INT CHECK (Age between 18 and 110),
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL,
)

CREATE TABLE [Status] (
	Id INT PRIMARY KEY IDENTITY,
	[Label] VARCHAR(30) NOT NULL
)

CREATE TABLE Reports (
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	StatusId INT FOREIGN KEY REFERENCES [Status](Id) NOT NULL,
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME,
	[Description] VARCHAR(200) NOT NULL,
	UserId INT FOREIGN KEY REFERENCES Users(Id)NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)

GO

-- DML
-- 2.Insert
INSERT INTO Employees 
(FirstName,LastName,Birthdate,DepartmentId)
VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports 
(CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
VALUES
(1,1,'2017-04-13',null,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07',null,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

-- 3.Update
UPDATE Reports
SET CloseDate = GETDATE()
WHERE CloseDate IS NULL

-- 4.Delete
DELETE FROM Reports
WHERE StatusId = 4

GO

-- 5.Unassigned Reports
SELECT r.[Description], FORMAT(r.OpenDate,'dd-MM-yyyy') AS OpenDate
FROM Reports AS r
WHERE r.EmployeeId is null
ORDER BY r.OpenDate ASC,  r.[Description]

-- 6.Reports & Categories
SELECT r.[Description], c.[Name]
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
ORDER BY r.[Description], c.[Name]

-- 7.Most Reported Category
SELECT c.[Name] AS CategoryName, COUNT(r.CategoryId) AS ReportsNumber
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
GROUP BY r.CategoryId,c.[Name]
ORDER BY ReportsNumber DESC, c.[Name]

-- 8.Birthday Report
SELECT u.Username, c.[Name] 
FROM Reports AS r
JOIN Users AS u ON u.Id = r.UserId
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE FORMAT(u.Birthdate,'dd-MM') = FORMAT(r.OpenDate,'dd-MM')
ORDER BY u.Username, c.[Name]

-- 9.Users per Employee
SELECT CONCAT(e.FirstName,' ',e.LastName) AS FullName, COUNT(u.Id) AS UsersCount
FROM Employees AS e
LEFT JOIN Reports AS r ON e.Id = r.EmployeeId
LEFT JOIN Users AS u ON u.Id = r.UserId
GROUP BY CONCAT(e.FirstName,' ',e.LastName)
ORDER BY UsersCount DESC, FullName

-- 10.Full Info
SELECT ISNULL(e.FirstName + ' ' + e.LastName,'None') AS Employee,
	   ISNULL(d.[Name],'None') AS Department,
       c.[Name] AS Category,
       r.[Description],
       FORMAT(r.OpenDate, 'dd.MM.yyyy') AS OpenDate,
       s.[Label] AS [Status],
       u.[Name] AS [User]
FROM Reports AS r
LEFT JOIN Employees AS e ON e.Id = r.EmployeeId
LEFT JOIN Categories AS c ON c.Id = r.CategoryId
LEFT JOIN Departments AS d ON d.Id = e.DepartmentId
LEFT JOIN [Status] AS s ON s.Id = r.StatusId
LEFT JOIN Users AS u ON u.Id = r.UserId
ORDER BY e.FirstName DESC, e.LastName DESC,
		 Department ,Category ,r.[Description],
         r.OpenDate ,[Status],[User] 

GO

-- 11.Hours to Complete
CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @result int
	IF @StartDate is null
		SET @result = 0
	ELSE IF @EndDate is null
		SET @result = 0
	ELSE
		SET @result = DATEDIFF(HOUR,@StartDate,@EndDate)
	RETURN @result
END

GO 

SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
  FROM Reports

GO

-- 12.Assign Employee
CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	BEGIN TRANSACTION
		DECLARE @EmpDepart INT = (SELECT DepartmentId FROM Employees WHERE Id = @EmployeeId)
		DECLARE @CategId INT = (SELECT CategoryId FROM Reports WHERE Id = @ReportId)
		DECLARE @ReportDepart INT = (SELECT DepartmentId FROM Categories WHERE Id = @CategId)
			IF (@EmpDepart <> @ReportDepart)
			BEGIN
				ROLLBACK;
				THROW 50001, 'Employee doesn''t belong to the appropriate department!', 1
			END

			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId
	COMMIT
END

GO

EXEC usp_AssignEmployeeToReport 30, 1
EXEC usp_AssignEmployeeToReport 17, 2