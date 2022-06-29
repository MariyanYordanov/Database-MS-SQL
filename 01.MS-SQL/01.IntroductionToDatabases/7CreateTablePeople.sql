-- 7 Create Table People
CREATE TABLE People 
(
	Id INT PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3,2),
	[Weight] DECIMAL(5,2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
);

INSERT INTO People (Id, [Name], Picture , Height, [Weight], Gender, Birthdate, Biography)
VALUES
(1 ,'Ivan Ivanov', 011100010100111010, 1.82, 103.23, 'm', '2001-01-15', 'I am a student in SoftUni'),
(2, 'Anna Petrova', 011100010100111010, 1.62, 53.12, 'f', '2002-11-23', 'I am a student in SoftUni'),
(3, 'Maria Ivanova', 011100010100111010, 1.70, 53.34, 'f', '2001-12-12', 'I am a student in SoftUni'),
(4, 'Stoyan Petrov', 011100010100111010, 1.72, 69.55, 'm', '2003-09-24', 'I am a student in iSoftUni'),
(5, 'Iva Stefanova', 011100010100111010, 1.64, 52.89, 'f', '2002-05-01', 'I am a student in SoftUni');