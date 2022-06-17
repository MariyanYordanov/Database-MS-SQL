-- Crearte database
CREATE DATABASE Bitbucket

USE Bitbucket

GO

-- 1.Database Design
CREATE TABLE Users (
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(30) NOT NULL,
	Email  VARCHAR(50) NOT NULL
)

CREATE TABLE Repositories (
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL  
)

CREATE TABLE RepositoriesContributors (
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	PRIMARY KEY (RepositoryId,ContributorId)
)

CREATE TABLE Issues (
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(255) NOT NULL,
	IssueStatus VARCHAR(6) NOT NULL,
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	AssigneeId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Commits (
	Id INT PRIMARY KEY IDENTITY,
	[Message] VARCHAR(255) NOT NULL,
	IssueId INT FOREIGN KEY REFERENCES Issues(Id),
	RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Files (
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	Size DECIMAL(16,2) NOT NULL,
	ParentId INT FOREIGN KEY REFERENCES Files(Id),
	CommitId INT FOREIGN KEY REFERENCES Commits(Id) NOT NULL
)

GO

-- 2. Insert
INSERT INTO Files
([Name],Size,ParentId,CommitId)
VALUES
('Trade.idk',2598.0,1,1),
('menu.net',9238.31,2,2),
('Administrate.soshy',1246.93,3,3),
('Controller.php', 7353.15, 4, 4),
('Find.java',9957.86,5,5),
('Controller.json',14034.87,3,6),
('Operate.xix',7662.92,7,7)

INSERT INTO Issues
(Title,IssueStatus,RepositoryId,AssigneeId)
VALUES
('Critical Problem with HomeController.cs file','open',1,4),
('Typo fix in Judge.html','open',4,3),
('Implement documentation for UsersService.cs','closed',8,2),
('Unreachable code in Index.cs','open',9,8)

-- 3.Update
UPDATE Issues
SET IssueStatus = 'closed' 
WHERE AssigneeId = 6

-- 4.Delete
DELETE FROM RepositoriesContributors
WHERE RepositoryId = (SELECT Id FROM Repositories WHERE [Name] = 'Softuni-Teamwork')

DELETE FROM Issues
WHERE RepositoryId = (SELECT Id FROM Repositories WHERE [Name] = 'Softuni-Teamwork')

-- 5.Commits
SELECT Id,[Message],RepositoryId,ContributorId
FROM Commits
ORDER BY Id,[Message],RepositoryId,ContributorId

-- 6.Front-end
SELECT Id,[Name],Size
FROM Files
WHERE Size > 1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC, Id, [Name]

-- 7.Issue Assignment
SELECT i.Id, CONCAT(u.Username,' : ',i.Title) 
FROM Issues AS i
JOIN Users AS u ON u.Id = i.AssigneeId
ORDER BY i.Id DESC, i.AssigneeId

-- 8.Single Files
SELECT fchild.Id, fchild.[Name], CONCAT(fchild.Size,'KB') AS Size
FROM Files AS fchild
LEFT JOIN Files AS fparent ON fchild.Id = fparent.ParentId
WHERE fparent.ParentId IS NULL
ORDER BY fchild.Id, fchild.[Name], Size DESC

-- 9.Commits in Repositories
-- Uncorrect conditions of the task
SELECT TOP(5) r.Id, r.[Name], COUNT(*) AS [Commits]
	FROM Commits c
	JOIN Repositories r ON c.RepositoryId = r.Id
	JOIN RepositoriesContributors rc ON rc.RepositoryId = r.Id
	GROUP BY r.Id, r.[Name]
	ORDER BY [Commits] DESC, r.Id, r.[Name]

-- 10.Average Size
SELECT u.Username,AVG(f.Size) AS Size
FROM Users AS u
JOIN Commits AS c ON c.ContributorId = u.Id
JOIN Files AS f ON f.CommitId = c.Id
GROUP BY u.Username
ORDER BY Size DESC, u.Username

GO

-- 11.All User Commits
CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(30))
RETURNS INT
AS
BEGIN
	RETURN (SELECT count(*) 
			FROM Commits AS c 
			JOIN Users AS u ON c.ContributorId = u.Id
			WHERE u.Username = @username)
END

GO

SELECT dbo.udf_AllUserCommits('UnderSinduxrein')

GO

-- 12. Search for Files
CREATE PROCEDURE usp_SearchForFiles(@fileExtension varchar(10))
AS
BEGIN
	SELECT f.Id, f.[Name], concat(f.Size, 'KB') AS Size
	FROM Files AS f
	WHERE RIGHT(f.[Name],LEN(@fileExtension)) = @fileExtension
	ORDER BY f.Id,f.[Name], Size DESC
END