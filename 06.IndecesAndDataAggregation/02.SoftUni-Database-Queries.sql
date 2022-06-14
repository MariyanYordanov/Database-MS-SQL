USE SoftUni
-- 13.Departments Total Salaries
  SELECT DepartmentID, SUM(Salary) AS TotalSalary
    FROM Employees
GROUP BY DepartmentID

-- 14.Employees Minimum Salaries
  SELECT DepartmentID, MIN(Salary) AS MinimumSalary
    FROM Employees
   WHERE HireDate > '2000-01-01' AND DepartmentID IN (2,5,7)
GROUP BY DepartmentID

-- 15.Employees Average Salaries
SELECT * 
  INTO EmployeesNewTable
  FROM Employees AS e
 WHERE e.Salary > 30000

DELETE
 FROM EmployeesNewTable
WHERE ManagerID = 42

UPDATE EmployeesNewTable
   SET Salary += 5000
 WHERE DepartmentID = 1

  SELECT DepartmentID,AVG(Salary) AS AverageSalary
    FROM EmployeesNewTable
GROUP BY DepartmentID

-- 16.Employees Maximum Salaries
  SELECT DepartmentID,MAX(Salary)
    FROM Employees
GROUP BY DepartmentID
  HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

-- 17.Employees Count Salaries
SELECT COUNT(*) AS Count
  FROM Employees AS e
 WHERE e.ManagerID IS NULL

-- 18.*3rd Highest Salary
   SELECT Ranked.DepartmentID,Ranked.Salary AS ThirdHighestSalary
FROM (SELECT e.DepartmentID,e.Salary,DENSE_RANK() OVER 
(PARTITION BY e.DepartmentID ORDER BY e.Salary DESC)
		        AS ThirdHighestSalary
		      FROM Employees AS e) AS Ranked
   WHERE Ranked.ThirdHighestSalary = 3
GROUP BY Ranked.DepartmentID, Ranked.Salary

-- 19.**Salary Challenge
SELECT FirstName,LastName,DepartmentID,Salary
  FROM Employees
 WHERE Salary > ( SELECT e.DepartmentID,AVG(Salary) AS avgs
				    FROM Employees AS e 
				GROUP BY e.DepartmentID)


--CREATE INDEX ID_Departments_DepartmentsID ON Departments DepartmentsID