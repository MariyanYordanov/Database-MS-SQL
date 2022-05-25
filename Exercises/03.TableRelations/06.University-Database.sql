--CREATE DATABASE [University];

USE [University];

CREATE TABLE [Subjects]
(
	[SubjectID] INT PRIMARY KEY IDENTITY,
	[SubjectName] VARCHAR(50) NOT NULL
);

CREATE TABLE [Majors] 
(
	[MajorID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE [Students]
(
	[StudentID] INT PRIMARY KEY IDENTITY,
	[StudentNumber] CHAR(10) NOT NULL,
	[StudentName] VARCHAR(50) NOT NULL,
	[MajorID] INT 
	CONSTRAINT FK_StudentMajors
	FOREIGN KEY ([MajorID])
	REFERENCES [Majors]([MajorID])
);

CREATE TABLE [Payments]
(
	[PaymentID] INT PRIMARY KEY IDENTITY,
	[PaymentDate] DATE NOT NULL,
	[PaymentAmount] DECIMAL (8,2) NOT NULL,
	[StudentID] INT
	CONSTRAINT FK_PaymentsStudents
	FOREIGN KEY ([StudentID])
	REFERENCES [Students]([StudentID])
);

CREATE TABLE [Agenda] 
(
	[StudentID] INT,
	[SubjectID] INT,

	CONSTRAINT PK_StudentsSubjects
	PRIMARY KEY ([StudentID],[SubjectID]),

	CONSTRAINT FK_StudentsSubjects_Students
	FOREIGN KEY ([StudentID])
	REFERENCES [Students]([StudentID]),

	CONSTRAINT FK_StudentsSubjects_Subjects
	FOREIGN KEY ([SubjectID])
	REFERENCES [Subjects]([SubjectID]),
);
