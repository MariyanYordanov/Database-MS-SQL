-- 14. Games from 2011 and 2012 year
USE Diablo;
SELECT TOP(50) [Name],
FORMAT([Start],'yyyy-MM-dd','bg-BG')
AS [Start]
FROM [Games]
WHERE DATEPART(YEAR,[Start]) >= '2011' 
AND DATEPART(YEAR,[Start]) <= '2012'
ORDER BY [Start] ASC;

-- 15. User Email Providers
SELECT [Username],
RIGHT([Email],LEN([Email]) - CHARINDEX('@',[Email])) 
AS [EmailProvider]
FROM [Users]
ORDER BY [EmailProvider],[Username];

-- 16. Get Users with IPAdress Like Pattern

SELECT [Username],[IpAddress] AS [IPAddress]
FROM [Users]
WHERE [IpAddress] LIKE '___.1%[0-9]%.%[0-9]%.___'
ORDER BY [Username] ASC;

-- 17. Show All Games with Duration

SELECT [Name]
	,CASE
		WHEN DATEPART(HOUR,[Start]) >= 0 
			AND DATEPART(HOUR,[Start]) < 12 
				THEN 'Morning'
		WHEN DATEPART(HOUR,[Start]) >= 12 
			AND DATEPART(HOUR,[Start]) < 18 
				THEN 'Afternoon'
		WHEN DATEPART(HOUR,[Start]) >= 18 
			AND DATEPART(HOUR,[Start]) < 24 
				THEN 'Evening'
	END AS [Part of the Day]
	,CASE
		WHEN [Duration] <= 3
				THEN 'Extra Short'
		WHEN [Duration] BETWEEN 4 AND 6
				THEN 'Short'
		WHEN [Duration] > 6
				THEN 'Long'
		WHEN [Duration] IS NULL
           THEN 'Extra Long'
	END AS [Duration]
FROM [Games]
ORDER BY [Name],[Duration],[Part of the Day] ASC;