-- 19 Basic Select All Fields

USE SoftUni
SELECT * FROM Towns;
SELECT * FROM Departments;
SELECT * FROM Employees;

-- 20 Basic Select All Fields and Order Them
USE SoftUni
SELECT * FROM Towns ORDER BY [Name] ASC;
SELECT * FROM Departments  ORDER BY [Name] ASC;
SELECT * FROM Employees  ORDER BY Salary DESC;

-- 21 Basic Select Some Fields
USE SoftUni
SELECT [Name] FROM Towns ORDER BY [Name] ASC;
SELECT [Name] FROM Departments  ORDER BY [Name] ASC;
SELECT FirstName, LastName, JobTitle, Salary 
FROM Employees  ORDER BY Salary DESC;

-- 22 Increase Employees Salary
USE SoftUni
UPDATE Employees
SET Salary *= 1.1;
SELECT Salary FROM Employees;

-- 23 Decrease Tax Rate
USE Hotel
UPDATE Payments
SET TaxRate *= 0.97;
SELECT TaxRate FROM Payments;

-- 24 Delete All Records
USE Hotel
DELETE Occupancies;