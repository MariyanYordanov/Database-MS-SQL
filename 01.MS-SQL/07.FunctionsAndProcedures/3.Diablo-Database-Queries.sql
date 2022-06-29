USE Diablo

GO
-- 13. Scalar Function: Cash in User Games Odd Rows
CREATE 
FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(50))
RETURNS TABLE
AS RETURN( SELECT SUM(Cash) AS SumCash
		    FROM (SELECT ug.Cash,ROW_NUMBER() OVER(ORDER BY ug.Cash DESC) AS r
		            FROM UsersGames ug
		       LEFT JOIN Games AS g ON ug.GameId = g.Id
		  		   WHERE g.[Name] = @gameName
				  )  AS sub
		    WHERE r % 2 <> 0
		  )			