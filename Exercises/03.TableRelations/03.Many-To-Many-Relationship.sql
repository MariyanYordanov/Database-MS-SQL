USE [Table-Relations]
CREATE TABLE [Students] 
(
	[StudentID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20) NOT NULL
);

CREATE TABLE [Exams]
(
	[ExamID] INT PRIMARY KEY IDENTITY(101,1),
	[Name] VARCHAR(20) NOT NULL
);

CREATE TABLE [StudentsExams] 
(
	[StudentID] INT,
	[ExamID] INT,

	CONSTRAINT PK_StudentSExams           -- Composite Primary Key
	PRIMARY KEY ([StudentID],[ExamID]),

	CONSTRAINT FK_StudentsExams_Students  -- Foreign Key to Students
	FOREIGN KEY ([StudentID])
	REFERENCES [Students]([StudentID]),

	CONSTRAINT FK_StudentsExams_Exams     -- Foreign Key to Exams
	FOREIGN KEY ([ExamID])
	REFERENCES [Exams]([ExamID])
);

INSERT INTO [Students]
VALUES
('Mila'),
('Toni'),
('Ron');

INSERT INTO [Exams]
VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g');

INSERT INTO [StudentsExams]
VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103);

SELECT * FROM [Students];
SELECT * FROM [Exams];
SELECT * FROM [StudentsExams];