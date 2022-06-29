 -- 12. Highest Peaks in Bulgaria
SELECT c.[CountryCode]
		,m.[MountainRange]
		,p.[PeakName]
		,p.[Elevation]
FROM [Countries] AS c
	JOIN [MountainsCountries] AS mc
		ON mc.CountryCode = c.[CountryCode]
	JOIN [Mountains] AS m
		ON m.Id = mc.MountainId
	JOIN [Peaks] AS p
		ON m.Id = p.MountainId
WHERE c.[CountryCode] = 'BG'
	AND [Elevation] > 2835
ORDER BY [Elevation] DESC


-- 13. Count Mountain Ranges Create a query that selects:
SELECT c.CountryCode, COUNT(m.[MountainRange]) AS [MountainRange]
FROM [Countries] AS c
	JOIN [MountainsCountries] AS mc 
		ON mc.[CountryCode] = c.[CountryCode]
	JOIN [Mountains] AS m 
		ON m.[Id] = mc.[MountainId]
WHERE c.CountryCode IN ('BG','US','RU')
GROUP BY c.CountryCode;


-- 14. Countries with Rivers
SELECT TOP(5) c.CountryName,r.RiverName
FROM [Countries] AS c
	LEFT JOIN [CountriesRivers] AS cr
		ON c.CountryCode =  cr.CountryCode
	LEFT JOIN [Rivers] AS r
		ON r.Id = cr.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName;

-- 15. *Continents and Currencies
SELECT k.ContinentCode,k.CurrencyCode,k.CurrencyUsage
FROM Continents AS c
JOIN (
	SELECT ContinentCode,CurrencyCode,COUNT(CurrencyCode) AS CurrencyUsage,
	DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS Rank
	FROM Countries  GROUP BY ContinentCode, CurrencyCode
	HAVING COUNT(CurrencyCode) > 1
) AS k ON c.ContinentCode = k.ContinentCode
WHERE k.Rank = 1


-- 16. Countries Without Any Mountains
SELECT COUNT(c.CountryCode)
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
WHERE m.MountainRange IS NULL


-- 17. Highest Peak and Longest River by Country
SELECT TOP(5) c.CountryName
		,MAX(p.Elevation) AS HighestPeakElevation
		,MAX(r.Length) AS LongestRiverLength
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = m.Id
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC,LongestRiverLength DESC,c.CountryName ASC

-- 18. Highest Peak Name and Elevation by Country
SELECT TOP(5) k.CountryName,k.[Highest Peak Name],k.[Highest Peak Elevation],k.Mountain
FROM (
		SELECT CountryName
		,ISNULL(p.PeakName,'(no highest peak)') AS [Highest Peak Name]
		,ISNULL(p.Elevation, 0) AS [Highest Peak Elevation]
		,ISNULL(m.MountainRange,'(no mountain)') AS [Mountain]
		,DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS Ranked
		FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
		LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainId = m.Id
	) AS k
WHERE k.Ranked = 1
