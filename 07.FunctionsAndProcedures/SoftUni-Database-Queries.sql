USE SoftUni
-- 1.Employees with Salary Above 35000
CREATE 
  PROC usp_GetEmployeesSalaryAbove35000
    AS 
 BEGIN
	   SELECT [FirstName],
			  [LastName]
	     FROM [Employees] 
		WHERE [Salary] > 35000
   END

    GO

EXEC dbo.usp_GetEmployeesSalaryAbove35000

-- 2.Employees with Salary Above Number
CREATE
  PROC usp_GetEmployeesSalaryAboveNumber @minSalary DECIMAL(18,4)
    AS
 BEGIN
	   SELECT FirstName,LastName
	     FROM Employees WHERE Salary >= @minSalary
   END

    GO

EXEC usp_GetEmployeesSalaryAboveNumber 48100

-- 3.Town Names Starting With
CREATE
  PROC usp_GetTownsStartingWith  @string VARCHAR(50)
    AS
 BEGIN
	   SELECT [Name]
	     FROM [Towns]
	    WHERE [Name] 
	     LIKE @string + '%'
	  --WHERE LEFT([Name], 1) = @string
  END

   GO

EXEC usp_GetTownsStartingWith 'b'

--4.Employees from Town
 CREATE
   PROC usp_GetEmployeesFromTown @townName VARCHAR(50)
     AS
  BEGIN
        SELECT [FirstName],
       		   [LastName]
          FROM [Employees]
		    AS [e]
     LEFT JOIN [Addresses]
	        AS [a]
            ON [e].[AddressID] = [a].[AddressID] 
     LEFT JOIN [Towns] 
	        AS [t]
            ON [t].[TownID] = [a].[TownID]
         WHERE [t].[Name] = @townName
	END

	 GO

EXEC usp_GetEmployeesFromTown 'Sofia'

-- 5.Salary Level Function
CREATE
FUNCTION ufn_GetSalaryLevel (@salary DECIMAL(18,4)) 
RETURNS NVARCHAR(7) 
AS
BEGIN
	DECLARE @salaryLevel NVARCHAR(7)	     IF @salary < 30000		SET @salaryLevel = 'Low'	ELSE IF @salary BETWEEN 30000 AND 50000		SET @salaryLevel = 'Average'	   ELSE	    SET @salaryLevel = 'High'	 RETURN @salaryLevelENDGOSELECT [Salary],[dbo].ufn_GetSalaryLevel([Salary]) AS [Salary Level]FROM [Employees]-- 6.Employees by Salary LevelCREATEPROC usp_EmployeesBySalaryLevel @level NVARCHAR(7)ASBEGIN	SELECT [FirstName],[LastName]	FROM [Employees]	WHERE [dbo].ufn_GetSalaryLevel([Salary]) = @levelENDGOEXEC usp_EmployeesBySalaryLevel 'High'EXEC usp_EmployeesBySalaryLevel 'Low'-- 7.Define FunctionCREATEFUNCTION  ufn_IsWordComprised(@setOfLetters NVARCHAR(50), @word NVARCHAR(50))RETURNS BIT ASBEGIN	DECLARE @result BIT = 1	DECLARE @index INT = 1	WHILE @index <= LEN(@word)		BEGIN			DECLARE @letter CHAR(1) = SUBSTRING(@word,@index,1)			IF @setOfLetters LIKE '%' + @letter + '%'				SET @result = 0			SET @index += 1		END	RETURN @resultENDGO