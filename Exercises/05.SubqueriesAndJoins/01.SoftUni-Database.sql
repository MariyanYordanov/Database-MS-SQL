-- 01. Employee Address
SELECT TOP(5)[Employees].EmployeeId
			,[Employees].JobTitle
			,[Employees].AddressId 
			,[Addresses].AddressText
FROM [Employees]
JOIN [Addresses] 
ON [Employees].AddressId = [Addresses].AddressID
ORDER BY [Addresses].AddressID;


-- 02. Addresses with Towns
SELECT TOP(50)[Employees].FirstName
			,[Employees].LastName
			,[Towns].[Name] AS [Town]
			,[Addresses].AddressText
FROM [Employees]
JOIN [Addresses] 
ON [Employees].AddressId = [Addresses].AddressID
JOIN [Towns]
ON [Addresses].TownID = [Towns].TownID
ORDER BY [FirstName],[LastName];


-- 03. Sales Employees
SELECT [Employees].EmployeeID
		,[Employees].FirstName
		,[Employees].LastName
		,[Departments].[Name] AS [DepartmentName]
FROM [Employees]
JOIN [Departments]
ON [Employees].DepartmentID = [Departments].DepartmentID
WHERE [Departments].[Name] = 'Sales'
ORDER BY [Employees].EmployeeID; 


-- 04. Employee Departments
SELECT TOP(5) [Employees].EmployeeID
			 ,[Employees].FirstName
			 ,[Employees].Salary
			 ,[Departments].[Name] AS [DepartmentName]
FROM [Employees]
JOIN [Departments]
ON [Employees].DepartmentID = [Departments].DepartmentID
WHERE [Employees].Salary > 15000
ORDER BY [Employees].DepartmentID;


-- 05. Employees Without Project
SELECT TOP(3) [Employees].EmployeeID
			,[Employees].FirstName
FROM [Employees] 
FULL JOIN [EmployeesProjects]
ON [Employees].EmployeeID = [EmployeesProjects].EmployeeID
WHERE [EmployeesProjects].EmployeeID IS NULL
ORDER BY [Employees].EmployeeID;


-- 06. Employees Hired After
SELECT [Employees].FirstName
		,[Employees].LastName
		,[Employees].HireDate
		,[Departments].[Name] AS [DeptName]
FROM [Employees]
FULL JOIN [Departments]
ON [Employees].DepartmentID = [Departments].DepartmentID
WHERE [Employees].HireDate > '1999-01-01'
AND [Departments].[Name] IN ('Sales','Finance')
ORDER BY [HireDate];


-- 07. Employees With Project
SELECT TOP(5) [Employees].EmployeeID
			,[Employees].FirstName
			,[Projects].[Name] AS [ProjectName]
FROM [Employees] 
JOIN [EmployeesProjects]
ON [Employees].EmployeeID = [EmployeesProjects].EmployeeID
JOIN [Projects]
ON  [EmployeesProjects].ProjectID = [Projects].ProjectID
WHERE [Projects].StartDate > '2002-08-13'
AND [Projects].EndDate IS NULL
ORDER BY [Employees].EmployeeID;


-- 08. Employee 24
SELECT [Employees].EmployeeID
		,[Employees].FirstName
		,[Projects].[Name] AS [ProjectName],
CASE 
	WHEN [Projects].StartDate > '2005-01-01' THEN NULL
	ELSE [Projects].[Name]
END
FROM [Employees]
FULL JOIN [EmployeesProjects]
ON [Employees].EmployeeID = [EmployeesProjects].EmployeeID
FULL JOIN [Projects]
ON  [EmployeesProjects].ProjectID = [Projects].ProjectID
WHERE [Employees].EmployeeID = 24;

-- 09. Employee Manager
SELECT e.EmployeeID 
		,e.FirstName 
		, e.ManagerID 
		, m.FirstName AS [ManagerName]
FROM [Employees] AS e
JOIN [Employees] AS m 
ON e.ManagerID = m.EmployeeID
WHERE M.EmployeeID IN (3,7)
ORDER BY e.EmployeeID

-- 10. Employee Summary
SELECT TOP(50) e.EmployeeID
			,e.FirstName + ' ' + e.LastName AS [EmployeeName]
			,m.FirstName + ' ' + m.LastName AS [ManagerName]
			,d.[Name] AS [DepartmentName]
FROM [Employees] AS e 
JOIN [Employees] AS m 
ON e.ManagerID = m.EmployeeID
JOIN [Departments] AS d
ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID

-- 11. Min Average Salary
SELECT TOP(1)
	(
		SELECT AVG([Salary])
		FROM [Employees] 
		WHERE [DepartmentID] = d.DepartmentID
	) AS [MinAverageSalary]
FROM [Departments] AS d
ORDER BY [MinAverageSalary];