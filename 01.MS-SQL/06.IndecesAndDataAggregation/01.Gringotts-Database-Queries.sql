USE Gringotts
-- 01.Records’ Count
SELECT COUNT(*) FROM WizzardDeposits AS [Count]

-- 02.Longest Magic Wand
SELECT MAX([MagicWandSize]) FROM WizzardDeposits AS [LongestMagicWand]

-- 03.Longest Magic Wand Per Deposit Groups
SELECT DepositGroup, MAX([MagicWandSize])
FROM WizzardDeposits AS [LongestMagicWand]
GROUP BY DepositGroup

-- 04.* Smallest Deposit Group Per Magic Wand Size
SELECT k.DepositGroup
FROM (
	SELECT TOP(2) DepositGroup,AVG([MagicWandSize]) AS AvgCol
	FROM WizzardDeposits AS [DepositGroup]
	GROUP BY DepositGroup
	ORDER BY DepositGroup DESC
	) AS k
ORDER BY k.DepositGroup

-- 05.Deposits Sum
SELECT DepositGroup, SUM([DepositAmount]) AS [TotalSum]
FROM WizzardDeposits
GROUP BY DepositGroup

-- 06.Deposits Sum for Ollivander Family
SELECT DepositGroup, SUM([DepositAmount]) AS [TotalSum]
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup

-- 07.Deposits Filter
SELECT DepositGroup, SUM([DepositAmount]) AS [TotalSum]
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM([DepositAmount]) < 150000
ORDER BY [TotalSum] DESC

-- 08.Deposit Charge
SELECT w.DepositGroup,w.MagicWandCreator,MIN(w.DepositCharge)
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup,w.MagicWandCreator
ORDER BY w.MagicWandCreator,w.DepositGroup

-- 09.Age Groups
SELECT ag.AgeGroup, COUNT(*)
FROM
	(
	SELECT 
	CASE
		WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
		END AS AgeGroup
	FROM WizzardDeposits
	) AS ag
GROUP BY ag.AgeGroup


-- 10.First Letter
SELECT LEFT(w.FirstName,1)  AS FirstLetter
FROM WizzardDeposits AS w
WHERE w.DepositGroup = 'Troll Chest'
GROUP BY LEFT(w.FirstName,1)
ORDER BY FirstLetter

-- 11.Average Interest 
SELECT w.DepositGroup
		,w.IsDepositExpired
		,AVG(w.DepositInterest) AS AverageInterest
FROM WizzardDeposits AS w
WHERE w.DepositStartDate > '1985-01-01'
GROUP BY  w.DepositGroup,w.IsDepositExpired
ORDER BY DepositGroup DESC,w.IsDepositExpired ASC

-- 12.* Rich Wizard, Poor Wizard
SELECT SUM(g.[Host Wizard Deposit]) - SUM(g.[Guest Wizard Deposit]) AS [SumDifference] --SUM( - LAST(g.[Guest Wizard Deposit]))
FROM (
	SELECT w.DepositAmount AS [Host Wizard Deposit],LEAD(w.DepositAmount,1) OVER ( ORDER BY w.DepositAmount) AS [Guest Wizard Deposit]
	FROM WizzardDeposits AS w
) AS g

SELECT w.DepositAmount AS [Host Wizard Deposit],LEAD(w.DepositAmount,1) OVER ( ORDER BY w.DepositAmount) AS [Guest Wizard Deposit]
	FROM WizzardDeposits AS w

