USE Bank

GO

-- 09. Find Full Name
CREATE PROC usp_GetHoldersFullName
AS
BEGIN
	SELECT 
	CONCAT(FirstName, ' ', LastName) AS [Full Name]
	FROM AccountHolders
END

GO
-- 10. People with Balance Higher Than
CREATE PROC usp_GetHoldersWithBalanceHigherThan(@amount MONEY)
AS
BEGIN
	SELECT ah.FirstName, ah.LastName
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @amount
	ORDER BY ah.FirstName, ah.LastName
END

EXEC usp_GetHoldersWithBalanceHigherThan 1000

GO

-- 11. Future Value Function
CREATE
FUNCTION ufn_CalculateFutureValue
		(@sum DECIMAL(15,4),
		 @yearlyInterestRate FLOAT, 
		 @numberOfYears INT)
RETURNS DECIMAL(15,4)
AS
BEGIN
	RETURN POWER((1 + @yearlyInterestRate),@numberOfYears) * @sum
END

SELECT dbo.ufn_CalculateFutureValue (1000, 0.1, 5) AS Result

GO
-- 12.Calculating Interest
CREATE OR ALTER PROC usp_CalculateFutureValueForAccount (@account INT, @interestRate FLOAT)
AS
BEGIN
	SELECT a.Id AS [Account Id]
		  ,FirstName AS [First Name]
		  ,LastName AS [Last Name]
		  ,Balance AS [Current Balance]
		  ,dbo.ufn_CalculateFutureValue(a.Balance, @interestRate, 5)
	FROM AccountHolders AS ah
	LEFT JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	WHERE a.Id = @account
END

EXEC dbo.usp_CalculateFutureValueForAccount 1,0.1