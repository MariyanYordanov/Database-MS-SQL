-- 12. Countries Holding ‘A’ 3 or More Times
USE [Geography];
SELECT [CountryName],[IsoCode]
FROM [Countries]
WHERE [CountryName] LIKE '%a%a%a%'
ORDER BY [IsoCode];		

-- 13. Mix of Peak and River Names
SELECT Peaks.PeakName
	,Rivers.RiverName,
	LOWER(CONCAT(LEFT(Peaks.PeakName, LEN(Peaks.PeakName)-1), Rivers.RiverName)) AS Mix
FROM Peaks
JOIN Rivers ON RIGHT(Peaks.PeakName, 1) = LEFT(Rivers.RiverName, 1)
ORDER BY Mix;